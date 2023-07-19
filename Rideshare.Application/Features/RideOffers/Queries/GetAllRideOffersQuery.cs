using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.Common;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetAllRideOffersQuery: PaginatedQuery, IRequest<PaginatedResponse<RideOfferListDto>>
{
}
