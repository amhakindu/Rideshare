
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Features.Auth.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Features.Auth.Handlers;

public sealed class GetUserByQueryHandler : IRequestHandler<GetUserByIdQuery, BaseResponse<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var applicationUser = await _userRepository.GetUserById(request.UserId);



        var user = new UserDto
        {

            FullName = applicationUser.FullName,
            PhoneNumber = applicationUser.PhoneNumber,
            Age = applicationUser.Age

        };
        if (applicationUser.LastLogin.HasValue && (DateTime.Now - applicationUser.LastLogin.Value).TotalDays < 30)
        {
            user.StatusByLogin = "ACTIVE";
        }
        else
        {
            user.StatusByLogin = "INACTIVE";
        }
        var roles = await _userRepository.GetUserRolesAsync(applicationUser);
        var roleDtos = _mapper.Map<List<RoleDto>>(roles);
        user.Roles.AddRange(roleDtos);

        var response = new BaseResponse<UserDto>();
        response.Success = true;
        response.Message = "Fetched In Successfully";
        response.Value = user;
        return response;

    }
}