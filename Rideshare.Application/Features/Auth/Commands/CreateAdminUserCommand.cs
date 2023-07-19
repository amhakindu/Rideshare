using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record CreateAdminUserCommand(): IRequest<BaseResponse<AdminUserDto>>
{ 
    public AdminCreationDto AdminCreationDto { get; set; }  
}