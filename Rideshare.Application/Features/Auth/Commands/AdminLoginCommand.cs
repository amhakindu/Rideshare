using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record AdminLoginCommand() : IRequest<BaseResponse<LoginResponse>>
{ 
 public LoginRequestByAdmin LoginRequest { get; set; }  
}