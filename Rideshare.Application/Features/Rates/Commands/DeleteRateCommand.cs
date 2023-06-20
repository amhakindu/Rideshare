using MediatR;
using Rideshare.Application.Common.Dtos.Rates;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Rates.Commands
{
	public class DeleteRateCommand : IRequest<BaseResponse<Unit>>
	{
		public int Id { get; set; }
		public string UserId { get; set; }
	}
}


