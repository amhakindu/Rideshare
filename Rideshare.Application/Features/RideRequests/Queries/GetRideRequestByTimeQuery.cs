using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestByTimeQuery : IRequest<BaseResponse<Dictionary<int,int>>>
{ 
    public RideRequestStatDto? RideRequestStatDto { get; set; }
}
