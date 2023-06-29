using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using static Rideshare.Application.Common.Constants.Utils;
using Rideshare.Domain.Common;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

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

    public async Task<PaginatedResponse<RideRequest>> GetAllRequests(int pageNumber = 1, int pageSize = 10)
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
    public async Task<Dictionary<int, int>> GetAllByGivenParameter(string? type, int? year, int? month)
    {
        Dictionary<int, int> rideRequests = new();
        if (type == "monthly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                 .Where(r => r.DateCreated.Year == year)
                 .GroupBy(item => item.DateCreated.Month)
                 .ToDictionaryAsync(g => g.Key, g => g.Count());

            for(int i = 1; i < 13; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }
        if (type == "weekly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                .Where(r => r.DateCreated.Year == year)
                .Where(r => r.DateCreated.Month == month)
                .GroupBy(r => r.DateCreated.Day / 7 + 1)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
          for(int i = 1; i < 6; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }
         if (type == "yearly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                 .GroupBy(item => item.DateCreated.Year)
                 .ToDictionaryAsync(g => g.Key, g => g.Count());

            for(int i = 2023; i < DateTime.Now.Year; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }

      
        return rideRequests;
    }

    public async Task<Dictionary<int, int>> GetRideRequestStatistics(string? type, int? year, int? month, Status status)
    {
        Dictionary<int, int> rideRequests = new();
        if (type == "monthly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                 .Where(r => r.Status == status)
                 .Where(r => r.DateCreated.Year == year)
                 .GroupBy(item => item.DateCreated.Month)
                 .ToDictionaryAsync(g => g.Key, g => g.Count());


                   for(int i = 1; i < 13; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }
        if (type == "weekly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                .Where(r => r.Status == status)
                .Where(r => r.DateCreated.Year == year)
                .Where(r => r.DateCreated.Month == month)
                .GroupBy(r => r.DateCreated.Day / 7 + 1)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            for(int i = 1; i < 6; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        }
        if (type == "yearly")
        {
            rideRequests = await _dbContext.Set<RideRequest>().AsNoTracking()
                 .Where(r => r.Status == status)
                 .GroupBy(item => item.DateCreated.Year)
          
               .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

         for(int i = 2023; i < DateTime.Now.Year; i++  ){
            if (!rideRequests.ContainsKey(i)){
                rideRequests.Add(i,0);
            }
            }
        return rideRequests;
    }


    public async Task<PaginatedResponse<RideRequest>> SearchByGivenParameter(int PageNumber, int PageSize, Status? status, double? fare, string? name, string? phoneNumber)
    {

        var response = new PaginatedResponse<RideRequest>();

        var rideRequestQuery = _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Include(rr => rr.Origin)
            .Include(rr => rr.Destination)
            .Include(rr => rr.User)
            .Where(item => (item.Status == (status ?? item.Status)))
            .Where(item => (item.User.PhoneNumber == (phoneNumber ?? item.User.PhoneNumber)))
            .Where(item => (item.CurrentFare <= (fare ?? item.CurrentFare)))
            .Where(item => (item.User.FullName == (name ?? item.User.FullName)));
        var count = await rideRequestQuery.CountAsync();
        var rideRequests = await rideRequestQuery.Skip((PageNumber - 1) * PageSize)
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        response.Paginated = rideRequests;
        response.Count = count;
        return response;

    }

    public async Task<Dictionary<string, Dictionary<int, int>>> GetAllByGivenStatus(string? type, int? year, int? month)
    {
        return new Dictionary<string, Dictionary<int, int>>(){
            {"failed", await GetRideRequestStatistics(type,year, month, Status.CANCELLED)},
            {"completed", await GetRideRequestStatistics(type,year, month, Status.COMPLETED)}
        };
    }



    public async Task<PaginatedResponse<RideRequest>> GetAllUserRequests(int PageNumber, int PageSize, string UserId)
    {

        var response = new PaginatedResponse<RideRequest>();
        var query = _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Where(r => r.UserId == UserId);
           
        var count = await query.CountAsync();
        var rideRequests = await query.Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        response.Count = count;
        response.Paginated = rideRequests;
        return response;

    }

    
}
