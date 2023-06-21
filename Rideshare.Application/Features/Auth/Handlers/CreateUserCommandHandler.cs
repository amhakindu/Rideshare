
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

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseResponse<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISmsSender _smsSender;
    private readonly IMapper _mapper;
    private static Random random = new Random();

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        
    }

    public async Task<BaseResponse<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        var roles = request.UserCreationDto.Roles;
        var applicationRoles = _mapper.Map<List<ApplicationRole>>(roles);


        var applicationUser = _mapper.Map<ApplicationUser>(request.UserCreationDto);

        var user = await _userRepository.CreateUserAsync(applicationUser, applicationRoles);
          var userDto = _mapper.Map<UserDto>(user);


        var response = new BaseResponse<UserDto>();
        response.Success = true;
        response.Message = "User Created Successfully";
        response.Value = userDto;
        return response;
    }
}