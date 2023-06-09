using MediatR;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestListQuery : IRequest<BaseResponse<PaginatedResponseDto<RideRequestDto>>>
{
    public SearchAndFilterDto? SearchAndFilterDto { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
