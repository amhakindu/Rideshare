
using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Commands
{
	public class UpdateRateCommand : IRequest<BaseResponse<int>>
	{
		public UpdateRateDto RateDto { get; set; }
	}
}
