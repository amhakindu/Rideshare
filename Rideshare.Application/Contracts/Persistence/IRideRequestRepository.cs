using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Domain.Common;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRideRequestRepository : IGenericRepository<RideRequest>
{
    Task<Dictionary<string, int>> GetTop5Commuter();
    Task<PaginatedResponse<RideRequest>> SearchByGivenParameter(int PageNumber, int PageSize, Status? status, double? fare, string? name, string? phoneNumber);
    Task<Dictionary<int, int>> GetAllByGivenParameter(string? type, int? year, int? month);
    Task<Dictionary<string, Dictionary<int, int>>> GetAllByGivenStatus(string? type, int? year, int? month);
    Task<PaginatedResponse<RideRequest>> GetAllRequests(int PageNumber, int PageSize);
    Task<PaginatedResponse<RideRequest>> GetAllUserRequests(int PageNumber, int PageSize, string UserId);


    Task<List<GeographicalLocation>> GetPopularDestinationsOfCommuter(string UserId, int limit);
    Task<RideRequest?> GetRideRequestWithDetail(int riderequestId);
    Task<Dictionary<string,int>> GetStatusStatistics();
}
