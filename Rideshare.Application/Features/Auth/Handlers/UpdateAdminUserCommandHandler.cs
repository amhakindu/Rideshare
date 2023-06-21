
using AutoMapper;
using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Application.Security.Handlers.CommandHandlers;

public sealed class UpdateAdminUserCommandHandler : IRequestHandler<UpdateAdminUserCommand, BaseResponse<AdminUserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateAdminUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<AdminUserDto>> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
    {
       
        var response = new BaseResponse<AdminUserDto>();
        var applicationUser = _mapper.Map<ApplicationUser>(request.UserId);


        var updatedUser = await _userRepository.UpdateUserAsync(request.UserId, applicationUser);

        var userDto = _mapper.Map<AdminUserDto>(updatedUser);

        response.Success = true;
        response.Message = "User Updated Successfully";
        response.Value = userDto;
        return response;
    }
}