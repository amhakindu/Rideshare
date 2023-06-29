using System.Collections.Generic;
using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Common.Dtos.Pagination;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetRideOffersOfDriverQuery: IRequest<BaseResponse<PaginatedResponseDto<RideOfferDto>>>
{
    public string UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
