using System;
using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetDriversStatisticsRequest : IRequest<BaseResponse<Dictionary<string, int>>>
    {
        public bool Weekly {get; set;}
        public  bool Monthly {get; set;}
        public bool Yearly {get; set;}
        
    }
}