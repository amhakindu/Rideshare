using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Common.Dtos.RideRequests;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestAllListQuery : PaginatedQuery, IRequest<PaginatedResponse<RideRequestDto>>
{
}
