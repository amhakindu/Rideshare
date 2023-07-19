using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record CreateDriverCommand(): IRequest<BaseResponse<UserDriverDto>>
{ 
    public DriverCreatingDto DriverCreatingDto { get; set; }  
}