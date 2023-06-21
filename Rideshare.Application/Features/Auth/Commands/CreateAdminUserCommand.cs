
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record CreateAdminUserCommand(): IRequest<BaseResponse<AdminUserDto>>
{ 
 public AdminCreationDto AdminCreationDto { get; set; }  
}
;