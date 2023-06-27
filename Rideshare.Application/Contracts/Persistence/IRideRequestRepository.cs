using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRideRequestRepository : IGenericRepository<RideRequest>
{
    Task<Dictionary<string, int>> GetTop5Commuter();
    Task<IReadOnlyList<RideRequest>> SearchByGivenParameter(int PageNumber, int PageSize,Status? status,int fare,string name ,string phoneNumber);
    Task<Dictionary<int,int>> GetAllByGivenParameter(string? type,int? year,int? month); 
    Task<Dictionary<string,int>> GetAllByGivenStatus(string? type,int? year,int? month,Status? status); 

}
