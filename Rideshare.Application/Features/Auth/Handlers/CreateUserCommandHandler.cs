
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
    private readonly IResourceManager _resourceManager;
    private static Random random = new Random();

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IResourceManager resourceManager)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _resourceManager = resourceManager;

    }

    public async Task<BaseResponse<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        var roles = request.UserCreationDto.Roles;
        var applicationRoles = _mapper.Map<List<ApplicationRole>>(roles);


        var applicationUser = _mapper.Map<ApplicationUser>(request.UserCreationDto);
        if (request.UserCreationDto.Profilepicture != null)
        {
            applicationUser.ProfilePicture = (await _resourceManager.UploadImage(request.UserCreationDto.Profilepicture)).AbsoluteUri;
        }

        var user = await _userRepository.CreateUserAsync(applicationUser, applicationRoles);

        var userDto = _mapper.Map<UserDto>(user);





        var response = new BaseResponse<UserDto>();
        response.Success = true;
        response.Message = "User Created Successfully";
        response.Value = userDto;
        return response;
    }
}