using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Common.Dtos.RideOffers;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetRideOffersOfDriverQuery: PaginatedQuery, IRequest<PaginatedResponse<RideOfferListDto>>
{
    public object Id { get; set; }
}
