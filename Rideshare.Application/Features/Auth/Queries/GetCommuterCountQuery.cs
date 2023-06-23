using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Queries
{
	public class GetCommuterCountQuery : IRequest<BaseResponse<CommuterStatusDto>>
	{
		public string option { get; set; } //yearly, monthly or weekly.
		public DateTime? Year { get; set; }
		public DateTime? Month { get; set; }
		public DateTime? Week { get; set; }

	}
}