using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRideOfferRepository: IGenericRepository<RideOffer>
{
    Task<IReadOnlyList<RideOffer>> GetRideOffersOfDriver(string DriverID);
}
