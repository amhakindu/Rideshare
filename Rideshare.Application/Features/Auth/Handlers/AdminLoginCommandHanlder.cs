
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Handlers;

public sealed class AdminLoginCommandHandler : IRequestHandler<AdminLoginCommand, BaseResponse<LoginResponse>>
{
    private readonly IUserRepository _userRepository;

    public AdminLoginCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async   Task<BaseResponse<LoginResponse>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {

        var result = await _userRepository.LoginByAdminAsync(request.LoginRequest.UserName, request.LoginRequest.Password);

        var response = new BaseResponse<LoginResponse>();
        response.Success = true;
        response.Message = "Logged In Successfully";
        response.Value = result;
        return response;
    }
}