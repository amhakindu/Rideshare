using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Tests.Commands;

public class UpdateRideRequestCommand : IRequest<BaseResponse<Unit>>
{
    public UpdateRideRequestDto RideRequestDto { get; set; }
    public string  UserId { get; set; }
}
