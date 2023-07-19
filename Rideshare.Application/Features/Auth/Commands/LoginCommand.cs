using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record LoginCommand() : IRequest<BaseResponse<LoginResponse>>
{ 
    public LoginRequest LoginRequest { get; set; }  
}
