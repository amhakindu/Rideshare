using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRideOfferRepository: IGenericRepository<RideOffer>
{
    Task<IReadOnlyList<RideOffer>> GetRideOffersOfDriver(string DriverID);
    Task<IReadOnlyList<ModelAndCountDto>> NoTopModelOffers();
   Task<List<DriverStatsDto>> GetTopDriversWithStats();


}
