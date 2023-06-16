using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideOffers;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetAllRideOffersQuery: IRequest<BaseResponse<IReadOnlyList<RideOfferListDto>>>
{
}
