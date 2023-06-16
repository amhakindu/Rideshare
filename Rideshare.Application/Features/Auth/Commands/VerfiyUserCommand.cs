using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Commands;

public record VerifyUserCommand() : IRequest<BaseResponse<bool>>
{
   public string UserId {get;set;}
   public VerifyRequest Code {get;set;}
}