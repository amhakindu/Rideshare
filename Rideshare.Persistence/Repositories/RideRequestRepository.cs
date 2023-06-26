using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using static Rideshare.Application.Common.Constants.Utils;

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
            .Include(ro => ro.Origin)
            .Include(ro => ro.Destination)
            .FirstOrDefault(ro => ro.Id == id);
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

        if(temp1 != null)
            entity.Origin = temp1;
        if(temp2 != null)
            entity.Destination = temp2;
        await _dbContext.SaveChangesAsync();
        return await Update(entity);
    }

    public async Task<IReadOnlyList<RideRequest>> GetAll(int pageNumber=1, int pageSize=10)
    {
        return await _dbContext.Set<RideRequest>()
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(ro => ro.Origin)
                .ThenInclude(loc => loc.Coordinate)
            .Include(ro => ro.Destination)
                .ThenInclude(loc => loc.Coordinate)
            .ToListAsync();
    }
}
