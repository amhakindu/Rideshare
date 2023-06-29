using MediatR;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Responses;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class SearchAndFilterQuery: IRequest<BaseResponse<PaginatedResponseDto<RideOfferDto>>>
{
    public SearchAndFilterDto SearchDto { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }   
}
