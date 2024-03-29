using MediatR;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Responses;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetRideOfferStatsQuery: TimeseriesQuery, IRequest<BaseResponse<Dictionary<int, int>>>
{
}
