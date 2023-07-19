using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Features.Common;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestListQuery : PaginatedQuery, IRequest<PaginatedResponse<RideRequestDto>>
{
    public RideRequestsListFilterDto? RideRequestsListFilterDto { get; set; }
}
