using MediatR;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Handlers;


public class VerifyUserHandler : IRequestHandler<VerifyUserCommand, BaseResponse<bool>>
{
    private readonly IUserRepository _userRepository;

    public VerifyUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<bool>> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);
        var response = new BaseResponse<bool>();

        if (user == null)
        {
            response.Success = false;
            response.Message = "Verification Failed";
            response.Value = false;
            return response;
        }




        if (request.Code.OTPCode == user.OtpCode)
        {
            user.IsVerified = true;
            await _userRepository.UpdateUserAsync(request.UserId, user);

            response.Success = true;
            response.Message = "Verification Succeeded";
            response.Value = true;
            return response;
        }
        response.Success = false;
        response.Message = "Verification Failed";
        response.Value = false;
        return response;
    }


}