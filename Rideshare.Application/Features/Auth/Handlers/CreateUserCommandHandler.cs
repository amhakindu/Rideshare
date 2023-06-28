
using AutoMapper;
using MediatR;
using Rideshare.Domain.Models;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;
using System.Text;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Common.Dtos.Security.Validators;
using Rideshare.Application.Exceptions;
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

        var validator = new UserCreationDtoValidators();
        
        var validationResult = await validator.ValidateAsync(request.UserCreationDto);


        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors.Select(q => q.ErrorMessage).ToList().First());

        var role = request.UserCreationDto.Roles;
        List<RoleDto> temp = new() ;
        temp.Add(role);
        
        var applicationRoles = _mapper.Map<List<ApplicationRole>>(temp);


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