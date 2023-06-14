using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Commands
{
	public class DeleteRateCommand : IRequest<BaseResponse<int>>
	{
		public int RateId { get; set; }
	}
}


