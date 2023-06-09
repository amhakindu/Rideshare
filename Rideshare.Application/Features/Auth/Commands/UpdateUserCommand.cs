

using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record UpdateUserCommand() :  IRequest<BaseResponse<UserDto>>

{
   public string UserId { get; set; }  
   public  UserUpdatingDto User { get; set; }  
}