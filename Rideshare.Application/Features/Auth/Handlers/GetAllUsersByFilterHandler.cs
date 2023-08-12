using MediatR;
using AutoMapper;
using Rideshare.Application.Responses; 
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Features.Auth.Queries;

namespace Rideshare.Application.Features.Auth.Handlers
{
    public sealed class GetUsersByFilterQueryHandler : IRequestHandler<GetUsersByFilterQuery, PaginatedResponse<UserDtoForAdmin>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersByFilterQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<UserDtoForAdmin>> Handle(GetUsersByFilterQuery request, CancellationToken cancellationToken)
        {
            var allApplicationUsers = await _userRepository.GetUsersAsync(request.PageNumber, request.PageSize);

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

                if (u.LastLogin.HasValue && (DateTime.UtcNow - u.LastLogin.Value).TotalDays < 30)
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

            var filteredUsers = usersWithRoles;

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                filteredUsers = filteredUsers.Where(u => u.PhoneNumber.StartsWith(request.PhoneNumber)).ToList();
            }

            if (!string.IsNullOrEmpty(request.RoleName))
            {
                filteredUsers = filteredUsers.Where(u => u.Roles.Any(r => r.Name.Contains(request.RoleName))).ToList();
            }

            if (!string.IsNullOrEmpty(request.FullName))
            {
                filteredUsers = filteredUsers.Where(u => u.FullName.StartsWith(request.FullName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                filteredUsers = filteredUsers.Where(u => u.StatusByLogin.Equals(request.Status)).ToList();
            }

            var response = new PaginatedResponse<UserDtoForAdmin>
            {
                Success = true,
                Message = "Users Fetched Successfully",
                Value = filteredUsers
            };

            return response;
        }
    }
}
