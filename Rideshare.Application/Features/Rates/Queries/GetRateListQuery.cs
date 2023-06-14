using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Queries

{
	public class GetRateListQuery : IRequest<BaseResponse<List<RateDto>>>
	{
		
	}
}
