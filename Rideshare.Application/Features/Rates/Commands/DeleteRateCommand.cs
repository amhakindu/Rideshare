using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Commands
{
	public class DeleteRateCommand : IRequest<BaseResponse<Unit>>
	{
		public int RateId { get; set; }
	}
}


