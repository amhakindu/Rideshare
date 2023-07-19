using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Commands
{
	public class CreateRateCommand :  IRequest<BaseResponse<int>>
	{
		public CreateRateDto RateDto { get; set; }
		public string UserId { get; set; }	
		
	}
}

