using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Queries

{
    public class GetUsersByRoleQuery : IRequest<BaseResponse<List<UserDto>>>
    {
        public string Role { get; set; }
    }


}