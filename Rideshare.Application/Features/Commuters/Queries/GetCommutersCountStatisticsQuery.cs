using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;

namespace Rideshare.Application.Features.Commuters.Queries;

public class GetCommutersCountStatisticsQuery: TimeseriesQuery, IRequest<BaseResponse<Dictionary<int, int>>>
{
}
