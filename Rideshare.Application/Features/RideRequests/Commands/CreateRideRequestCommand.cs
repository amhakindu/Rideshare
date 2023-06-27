using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Commands;

public class CreateRideRequestCommand : IRequest<BaseResponse<Dictionary<string, object>>>
{
    public CreateRideRequestDto? RideRequestDto { get; set; }
}
