using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestListQuery : IRequest<BaseResponse<List<RideRequestDto>>>
{
    public Status? status {get;set;}
    public int fare {get;set;}
    public string? name {get;set;} 
    public string? phoneNumber{get;set;}
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
