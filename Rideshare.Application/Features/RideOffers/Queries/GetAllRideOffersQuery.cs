using System.Collections.Generic;
using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Common.Dtos.Pagination;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetAllRideOffersQuery: IRequest<BaseResponse<PaginatedResponseDto<RideOfferDto>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
