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

    public async Task<IReadOnlyList<RideOffer>> GetRideOffersOfDriver(string DriverID){
        return (IReadOnlyList<RideOffer>) _dbContext.RideOffers
            .Where(rideOffer => rideOffer.DriverID == DriverID).ToList();
    }
}
