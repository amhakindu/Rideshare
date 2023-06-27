using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Contracts.Persistence;

public interface IRideRequestRepository : IGenericRepository<RideRequest>
{
    Task<Dictionary<string, int>> GetTop5Commuter();
    Task<Dictionary<string,object>> SearchByGivenParameter(int PageNumber, int PageSize,Status? status,int? fare,string? name ,string? phoneNumber);
    Task<Dictionary<int,int>> GetAllByGivenParameter(string? type,int? year,int? month); 
    Task<Dictionary<string,Dictionary<int, int>>> GetAllByGivenStatus(string? type,int? year,int? month); 
    Task<Dictionary<string,object>> GetAllRequests(int PageNumber, int PageSize);
    Task<Dictionary<string,object>> GetAllUserRequests(int PageNumber, int PageSize,string UserId);


}
