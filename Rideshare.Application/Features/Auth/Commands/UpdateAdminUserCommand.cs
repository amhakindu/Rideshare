using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record UpdateAdminUserCommand() :  IRequest<BaseResponse<AdminUserDto>>

{
   public string UserId { get; set; }  
   public  AdminUpdatingDto UpdatingDto { get; set; }  
}