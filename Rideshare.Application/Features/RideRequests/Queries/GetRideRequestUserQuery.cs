using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Common.Dtos.RideRequests;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestUserQuery : PaginatedQuery, IRequest<PaginatedResponse<RideRequestDto>>
{
    public string? UserId { get; set; }
}
