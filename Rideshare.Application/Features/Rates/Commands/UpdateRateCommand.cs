
using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Commands
{
	public class UpdateRateCommand : IRequest<BaseResponse<Unit>>
	{
		public UpdateRateDto RateDto { get; set; }
		public string UserId { get; set; }	
	}
}
