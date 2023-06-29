using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRideRequestRepository : IGenericRepository<RideRequest>
{
    Task<Dictionary<string, int>> GetTop5Commuter();
    Task<List<GeographicalLocation>> GetPopularDestinationsOfCommuter(string UserId, int limit);
    Task<RideRequest?> GetRideRequestWithDetail(int riderequestId);
    Task<PaginatedResponse<RideRequest>> GetAll(int pageNumber=1, int pageSize=10);

}
