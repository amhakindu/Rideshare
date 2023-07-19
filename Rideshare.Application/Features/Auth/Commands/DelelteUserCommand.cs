using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Commands;

public sealed record DeleteUserCommand(): IRequest<BaseResponse<Double>>
{ 
    public string UserId { get; set; }  
}