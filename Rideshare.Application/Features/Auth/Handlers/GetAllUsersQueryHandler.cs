
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

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, BaseResponse<List<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var allApplicationUsers = await _userRepository.GetUsersAsync();


        var usersWithRoles = new List<UserDto>();
        foreach (var u in allApplicationUsers)
        {
            var user = new UserDto
            {

                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                Age = u.Age,
                
                

            };
            if (u.LastLogin.HasValue && (DateTime.Now - u.LastLogin.Value).TotalDays < 30)
            {
                user.StatusByLogin = "ACTIVE";
            }
            else
            {
                user.StatusByLogin = "INACTIVE";
            }
            var roles = await _userRepository.GetUserRolesAsync(u);
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);
            user.Roles.AddRange(roleDtos);
            usersWithRoles.Add(user);
        }
        var response = new BaseResponse<List<UserDto>>();
        response.Success = true;
        response.Message = "Fetched In Successfully";
        response.Value = usersWithRoles;
        return response;

    }
}