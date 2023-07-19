using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetRideOfferStatsWithStatusQuery: TimeseriesQuery, IRequest<BaseResponse<Dictionary<string, Dictionary<int, int>>>>
{
}
