using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRideOfferRepository: IGenericRepository<RideOffer>
{
    Task<Dictionary<string, object>> GetAllPaginated(int pageNumber=1, int pageSize=10);
    Task<Dictionary<string, object>> GetRideOffersOfDriver(int DriverID, int PageNumber=1, int PageSize=10);
    Task<IReadOnlyList<RideOffer>> GetActiveRideOffers();
    Task<RideOffer?> GetActiveRideOfferOfDriver(int DriverId);
    Task<RideOffer> AcceptRideRequest(int riderequestId);
    Task<int> CancelRideOffer(int rideOfferId);
    Task<List<GeographicalLocation>> GetPopularDestinationsOfDriver(int driverId, int limit);
    Task<IReadOnlyList<ModelAndCountDto>> NoTopModelOffers();
    Task<List<DriverStatsDto>> GetTopDriversWithStats();
    Task<int> UpdateCurrentLocation(RideOffer rideoffer, GeographicalLocation location);
    Task<Dictionary<string, object>> SearchAndFilter(double MinCost, double MaxCost, string? driverName, string? driverPhoneNumber, Status? status, int PageNumber=1, int PageSize=10);
    Task<Dictionary<int, int>> GetRideOfferStatistics(int? year, int? month, Status? status);
    Task<Dictionary<string, int>> GetRideOfferCountForEachStatus();
    Task<Dictionary<string, Dictionary<int, int>>> GetRideOfferStatisticsWithStatus(int? year, int? month);
}
