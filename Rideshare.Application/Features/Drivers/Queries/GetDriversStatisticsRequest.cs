using System;
using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetDriversStatisticsRequest : IRequest<BaseResponse<Dictionary<int, int>>>
    {
        public string TimeFrame {get; set;}
        public int Year;
        public int Month;

    }
}