using System;
using MediatR;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Queries;

public class GetDriversStatisticsRequest : TimeseriesQuery, IRequest<BaseResponse<Dictionary<int, int>>>
{
}
