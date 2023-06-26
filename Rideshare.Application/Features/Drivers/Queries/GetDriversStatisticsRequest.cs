using System;
using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetDriversStatisticsRequest : IRequest<BaseResponse<Dictionary<int, Dictionary<string, int>>>>
    {
        public string TimeFrame {get; set;}
    }
}