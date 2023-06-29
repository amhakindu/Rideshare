using MediatR;
using Rideshare.Application.Common.Dtos.Statistics;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetPercentageChangeFromLastWeekQuery: IRequest<BaseResponse<IList<EntityCountChangeDto>>>
{
}
