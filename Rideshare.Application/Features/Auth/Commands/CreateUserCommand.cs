using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;


namespace Rideshare.Application.Features.Auth.Commands;

public sealed record CreateUserCommand(): IRequest<BaseResponse<UserDto>>
{ 
    public UserCreationDto UserCreationDto { get; set; }  
}
