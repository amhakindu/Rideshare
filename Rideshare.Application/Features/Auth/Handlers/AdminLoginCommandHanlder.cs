using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Features.Auth.Commands;

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
        response.Message = "Logged In Successfully";
        response.Success = true;
        response.Value = result;
        return response;
    }
}