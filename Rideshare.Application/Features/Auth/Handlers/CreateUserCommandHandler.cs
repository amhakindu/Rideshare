
using AutoMapper;
using MediatR;
using Rideshare.Domain.Models;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;
using System.Text;
using Rideshare.Application.Contracts.Services;

namespace Rideshare.Application.Features.Auth.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseResponse<ApplicationUser>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISmsSender _smsSender;
    private readonly IMapper _mapper;
    private static Random random = new Random();

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper ,ISmsSender smsSender )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _smsSender = smsSender;
    }

    public async Task<BaseResponse<ApplicationUser>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var password = request.UserCreationDto.Password;

        var roles = request.UserCreationDto.Roles;
        var applicationRoles = _mapper.Map<List<ApplicationRole>>(roles);


        var applicationUser = _mapper.Map<ApplicationUser>(request.UserCreationDto);

        var user = await _userRepository.CreateUserAsync(applicationUser, password, applicationRoles);

        // var code = GenerateCode();

        // user.OtpCode = code;

        // // Save the OTP code in a secure storage (database, cache, etc.)

        // await _smsSender.SendSmsAsync(user.PhoneNumber, $"Your OTP code is: {code}");

        // await _userRepository.UpdateUserAsync(user.Id,user);


        var response = new BaseResponse<ApplicationUser>();
        response.Success = true;
        response.Message = "User Created Successfully";
        response.Value = user;
        return response;
    }

     public static string GenerateCode()
    {
        StringBuilder codeBuilder = new StringBuilder();

        for (int i = 0; i < 4; i++)
        {
            int digit = random.Next(0, 10); // Generate a random digit between 0 and 9
            codeBuilder.Append(digit);
        }

        return codeBuilder.ToString();
    }
}