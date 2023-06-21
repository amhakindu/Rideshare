using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestQuery : IRequest<BaseResponse<RideRequestDto>>
{
    public int Id { get; set; }
    public string UserId { get; set; }
}
