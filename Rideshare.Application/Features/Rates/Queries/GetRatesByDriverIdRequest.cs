using System;
using MediatR;
using Rideshare.Application.Common.Dtos;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetRatesByDriverIdRequest : IRequest<BaseResponse<List<RateDto>>>
    {
        public int DriverId {get; set;}
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}