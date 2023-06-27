using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideRequests;

public class SearchAndFilterDto
{
    public Status? status {get;set;}
    public int? fare {get;set;}
    public string? name {get;set;} 
    public string? phoneNumber{get;set;}

}
