using MediatR;
using AutoMapper;
using Rideshare.Domain.Models;
using Rideshare.Domain.Entities;
using Rideshare.Application.Responses;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Common.Dtos.Drivers.Validators;
using Rideshare.Application.Common.Dtos.Security.Validators;

namespace Rideshare.Application.Features.Auth.Handlers;

public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, BaseResponse<UserDriverDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IResourceManager _resourceManager;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDriverCommandHandler(IUserRepository userRepository, IMapper mapper, IResourceManager resourceManager, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _resourceManager = resourceManager;
        _unitOfWork = unitOfWork;
    }
    public async Task<BaseResponse<UserDriverDto>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {

        var userValidator = new DriverCreationDtoValidators();

        var validationResultForUser = await userValidator.ValidateAsync(request.DriverCreatingDto);
        if (!validationResultForUser.IsValid)
            throw new ValidationException(validationResultForUser.Errors.Select(q => q.ErrorMessage).ToList().First());

        var role = new RoleDto
        {
            Id = "9f4ca49c-f74f-4a97-b90c-b66f40eb9a5f",
            Name = "Driver"
        };

      
        List<RoleDto> temp = new();
        temp.Add(role);

        var applicationRoles = _mapper.Map<List<ApplicationRole>>(temp);


        var applicationUser = _mapper.Map<ApplicationUser>(request.DriverCreatingDto);
        if (request.DriverCreatingDto.Profilepicture != null)
        {
            applicationUser.ProfilePicture = (await _resourceManager.UploadImage(request.DriverCreatingDto.Profilepicture)).AbsoluteUri;
        }
       
        var driver = _mapper.Map<Driver>(request.DriverCreatingDto.DriverDto);
        driver.License = (await _resourceManager.UploadImage(request.DriverCreatingDto.DriverDto.License)).AbsoluteUri;
        
        var user = await _userRepository.CreateUserAsync(applicationUser, applicationRoles);

        driver.UserId = user.Id;

        if (await _unitOfWork.DriverRepository.Add(driver) == 0)
            throw new InternalServerErrorException("Database Error: Unable To Save");

        var userDto = _mapper.Map<UserDto>(user);

        var response = new BaseResponse<UserDriverDto>();

        var userDriverDto = new UserDriverDto
        {
            FullName = userDto.FullName,
            PhoneNumber = userDto.PhoneNumber,
            Age = userDto.Age,
            StatusByLogin = userDto.StatusByLogin,
            DriverId = driver.Id,
            UserId = user.Id
        };
        response.Success = true;
        response.Message = "Driver Created Successfully";
        response.Value = userDriverDto;
        return response;
    }
}