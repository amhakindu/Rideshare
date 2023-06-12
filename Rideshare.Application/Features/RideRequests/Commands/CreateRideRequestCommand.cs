using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Tests.Commands;

public class CreateRideRequestCommand : IRequest<BaseResponse<int>>
{

    public CreateRideRequestDto RideRequestDto { get; set; }
}
