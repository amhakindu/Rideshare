using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Queries
{
    public class GetRateDetailQuery : IRequest<BaseResponse<RateDto>>
    {
        public int RateId { get; set; }
    }
}