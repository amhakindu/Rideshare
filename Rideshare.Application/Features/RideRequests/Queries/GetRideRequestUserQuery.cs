using MediatR;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestUserQuery : IRequest<BaseResponse<PaginatedResponseDto<RideRequestDto>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? UserId { get; set; }
}
