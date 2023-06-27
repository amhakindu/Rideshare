
using AutoMapper;
using MediatR;
using Rideshare.Domain.Models;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.Security;
using System.Text;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Common.Dtos.Drivers.Validators;
using Rideshare.Domain.Entities;
using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.Auth.Handlers;

public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, BaseResponse<UserDriverDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISmsSender _smsSender;
    private readonly IMapper _mapper;
    private readonly IResourceManager _resourceManager;
    private readonly IUnitOfWork _unitOfWork;
    private static Random random = new Random();

    public CreateDriverCommandHandler(IUserRepository userRepository, IMapper mapper, IResourceManager resourceManager, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _resourceManager = resourceManager;
        _unitOfWork = unitOfWork;

    }

    public async Task<BaseResponse<UserDriverDto>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {

        var role = request.DriverCreatingDto.Roles;
        List<RoleDto> temp = new();
        temp.Add(role);

        var applicationRoles = _mapper.Map<List<ApplicationRole>>(temp);


        var applicationUser = _mapper.Map<ApplicationUser>(request.DriverCreatingDto);
        if (request.DriverCreatingDto.Profilepicture != null)
        {
            applicationUser.ProfilePicture = (await _resourceManager.UploadImage(request.DriverCreatingDto.Profilepicture)).AbsoluteUri;
        }

        var user = await _userRepository.CreateUserAsync(applicationUser, applicationRoles);

        var validator = new CreateDriverDtoValidator();

        var validationResult = await validator.ValidateAsync(request.DriverCreatingDto.DriverDto);


        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors.Select(q => q.ErrorMessage).ToList().First());




        var driver = _mapper.Map<Driver>(request.DriverCreatingDto.DriverDto);

        driver.UserId = user.Id;
        driver.License = (await _resourceManager.UploadImage(request.DriverCreatingDto.DriverDto.License)).AbsoluteUri;

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
        response.Message = "Driver User Created Successfully";
        response.Value = userDriverDto;
        return response;
    }
}