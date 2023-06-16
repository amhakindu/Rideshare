
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record LoginCommand() : IRequest<BaseResponse<LoginResponse>>
{ 
 public LoginRequest LoginRequest { get; set; }  
}
;
