using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public class RideRequestsListFilterDto
{
    public Status? status {get;set;}
    public double? fare {get;set;}
    public string? name {get;set;} 
    public string? phoneNumber{get;set;}

}
