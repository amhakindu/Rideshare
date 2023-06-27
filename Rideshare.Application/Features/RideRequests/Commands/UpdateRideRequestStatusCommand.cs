using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Commands;

public class UpdateRideRequestStatusCommand : IRequest<BaseResponse<Unit>>
{
    public int Id{ get; set; }
    public string? UserId { get; set; }
}
