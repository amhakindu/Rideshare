using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Auth.Commands;
using Rideshare.Application.Features.Auth.Queries;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;
using System.Linq;

namespace Rideshare.Application.Features.Auth.Handlers
{
    public sealed class GetUsersByFilterQueryHandler : IRequestHandler<GetUsersByFilterQuery, BaseResponse<PaginatedUserList>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersByFilterQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<PaginatedUserList>> Handle(GetUsersByFilterQuery request, CancellationToken cancellationToken)
        {
            var allApplicationUsers = await _userRepository.GetUsersAsync(request.PageNumber, request.PageSize);

            var usersWithRoles = new List<UserDtoForAdmin>();
            foreach (var u in allApplicationUsers.PaginatedUsers)
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

            var filteredUsers = usersWithRoles;

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                filteredUsers = (List<UserDtoForAdmin>)filteredUsers.Where(u => u.PhoneNumber.Contains(request.PhoneNumber));
            }

            if (!string.IsNullOrEmpty(request.RoleName))
            {
                filteredUsers = (List<UserDtoForAdmin>)filteredUsers.Where(u => u.Roles.Any(r => r.Name.Contains(request.RoleName)));
            }

            if (!string.IsNullOrEmpty(request.FullName))
            {
                filteredUsers = (List<UserDtoForAdmin>)filteredUsers.Where(u => u.FullName.Contains(request.FullName));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                filteredUsers = (List<UserDtoForAdmin>)filteredUsers.Where(u => u.StatusByLogin.Equals(request.Status));
            }

            var paginatedResponse = new PaginatedUserList
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Count = filteredUsers.Count(),
                PaginatedUsers = filteredUsers.ToList()
            };

            var response = new BaseResponse<PaginatedUserList>
            {
                Success = true,
                Message = "Fetched Successfully",
                Value = paginatedResponse
            };

            return response;
        }
    }
}
