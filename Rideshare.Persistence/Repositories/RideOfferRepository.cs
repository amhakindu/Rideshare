using Microsoft.EntityFrameworkCore;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Repositories;

public class RideOfferRepository : GenericRepository<RideOffer>, IRideOfferRepository
{
    private readonly RideshareDbContext _dbContext;
    public RideOfferRepository(RideshareDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<RideOffer>> GetRideOffersOfDriver(string DriverID, int PageNumber, int PageSize){
        return await _dbContext.Set<RideOffer>().Where(x => x.DriverID == DriverID)
            .AsNoTracking()
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
}
