using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Tests.Commands;

public class DeleteRideRequestCommand : IRequest<BaseResponse<Unit>>
{
    public int Id{ get; set; } 
    public string  UserId { get; set; }
}
