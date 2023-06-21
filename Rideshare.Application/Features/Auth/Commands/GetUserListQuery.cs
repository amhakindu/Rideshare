using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Commands

{
    public class GetUserListQuery : IRequest<BaseResponse<List<UserDto>>>
	{
		public UserDto UserDto { get; set; }
    }
}
