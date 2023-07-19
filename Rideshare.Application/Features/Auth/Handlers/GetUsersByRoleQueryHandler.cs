using MediatR;
using AutoMapper;
using Rideshare.Application.Responses;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Features.Auth.Queries;

namespace Rideshare.Application.Features.Auth.Handlers;

public sealed class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery,  PaginatedResponse<UserDtoForAdmin>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUsersByRoleQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<UserDtoForAdmin>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
    {
        var allApplicationUsers = await _userRepository.GetUsersByRoleAsync(request.Role,request.PageNumber,request.PageSize);


        var usersWithRoles = new List<UserDtoForAdmin>();
        foreach (var u in allApplicationUsers.Value)
        {
            var user = new UserDtoForAdmin
            {

                Id = u.Id,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                Age = u.Age

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

        return new PaginatedResponse<UserDtoForAdmin>(){
            Message= "Users Fetched Successfully",
            Value= usersWithRoles,
            Count= allApplicationUsers.Count,
            PageNumber= request.PageNumber,
            PageSize= request.PageSize
        };

    }
}