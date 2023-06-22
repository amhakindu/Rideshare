
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record AdminLoginCommand() : IRequest<BaseResponse<LoginResponse>>
{ 
 public LoginRequestByAdmin LoginRequest { get; set; }  
}
;
