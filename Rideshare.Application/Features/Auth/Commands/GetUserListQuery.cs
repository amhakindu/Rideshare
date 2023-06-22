using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Commands

{
    public class GetUserListQuery : IRequest<BaseResponse<List<UserDto>>>
	{
		public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
