using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using static Rideshare.Application.Common.Constants.Utils;
using Rideshare.Domain.Common;
using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.Persistence.Repositories;

public class RideRequestRepository : GenericRepository<RideRequest>, IRideRequestRepository
{
    private const double RADIUS_IN_METERS = 150.0;
    private readonly RideshareDbContext _dbContext;
    
    public RideRequestRepository(RideshareDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RideRequest?> Get(int id)
    {
        return _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Include(rr => rr.Origin)
            .Include(rr => rr.Destination)
            .FirstOrDefault(rr => rr.Id == id);
    }
    public async Task<int> Add(RideRequest entity)
    {
        var locations = await _dbContext.Locations.ToListAsync();
        var temp1 = locations.OrderBy(g => HaversineDistance(g.Coordinate, entity.Origin.Coordinate))
            .FirstOrDefault();
        var temp2 = locations.OrderBy(g => HaversineDistance(g.Coordinate, entity.Destination.Coordinate))
            .FirstOrDefault();
        
        if (temp1 != null && HaversineDistance(temp1.Coordinate, entity.Origin.Coordinate) <= RADIUS_IN_METERS)
            entity.Origin = null;
        if (temp2 != null && HaversineDistance(temp2.Coordinate, entity.Destination.Coordinate) <= RADIUS_IN_METERS)
            entity.Destination = null;

        await _dbContext.AddAsync(entity);

        if(entity.Origin == null)
            entity.Origin = temp1;
        if(entity.Destination == null)
            entity.Destination = temp2;
        await _dbContext.SaveChangesAsync();
        return await Update(entity);
    }

    public async Task<PaginatedResponse<RideRequest>> GetAll(int pageNumber=1, int pageSize=10)
    {
        var response = new PaginatedResponse<RideRequest>();
        
        var rideRequests = await _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(rr => rr.Origin)
            .Include(rr => rr.Destination)
            .ToListAsync();
        response.Count = await _dbContext.RideRequests.CountAsync();
        response.Paginated = rideRequests;
        return response;
    }
    public async Task<Dictionary<string, int>> GetTop5Commuter()
    {
        var topUsers = await _dbContext.Set<RideRequest>()
            .GroupBy(rideRequest => rideRequest.UserId)
            .Select(group => new
            {
                UserId = group.Key,
                RideRequestCount = group.Count()
            })
            .OrderByDescending(user => user.RideRequestCount)
            .Take(5)
            .ToDictionaryAsync(user => user.UserId, user => user.RideRequestCount);
        return topUsers;
    }

    public Task<List<GeographicalLocation>> GetPopularDestinationsOfCommuter(string UserId, int limit)
    {
        return _dbContext.Set<RideRequest>()
            .Where(riderequest => riderequest.UserId == UserId)
            .GroupBy(riderequest => riderequest.Destination)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<RideRequest?> GetRideRequestWithDetail(int riderequestId)
    {
        return await _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Include(rr => rr.Origin)
            .Include(rr => rr.Destination)
            .Include(rr => rr.User)
            .FirstOrDefaultAsync(rr => rr.Id == riderequestId);
    }
}
