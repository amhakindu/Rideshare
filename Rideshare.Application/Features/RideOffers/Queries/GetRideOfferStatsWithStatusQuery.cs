using MediatR;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Responses;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetRideOfferStatsWithStatusQuery: IRequest<BaseResponse<Dictionary<string, Dictionary<int, int>>>>
{
    public RideOfferStatsDto StatsDto { get; set; }
}
