using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Common.Dtos.Rates;

namespace Rideshare.Application.Features.Drivers.Queries
{
    public class GetRatesByDriverIdRequest : PaginatedQuery, IRequest<BaseResponse<List<RateDto>>>
    {
        public int DriverId {get; set;}
    }
}