using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Responses;
using Rideshare.Domain.Models;

namespace Rideshare.Application.Features.Auth.Queries
{
    public class GetCommuterStatusQueryHandler : IRequestHandler<GetCommuterStatusQuery, BaseResponse<CommuterStatusDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetCommuterStatusQueryHandler(IUserRepository userRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<CommuterStatusDto>> Handle(GetCommuterStatusQuery request, CancellationToken cancellationToken)
        {
            var currentTime = DateTime.Now;
            var loginThreshold = currentTime.AddMinutes(-request.TimeIntervalMinutes);
           //loginThreshold is a datetime in past such that any user logged in after it considered as active.
            var users = await _userRepository.GetUsersAsync();
            var activeCommuters = 0;
            var idleCommuters = 0;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Commuter"))
                {
                    // if (user.LastLoginTime >= loginThreshold)
                    //     activeCommuters++;
                    // else
                        idleCommuters++;
                }
            }

            var commuterStatusDto = new CommuterStatusDto
            {
                ActiveCommuters = activeCommuters,
                IdleCommuters = idleCommuters
            };

            var response = new BaseResponse<CommuterStatusDto>
            {
                Success = true,
                Message = "active & idle commuters counted successfully",
                Value = commuterStatusDto
            };

            return response;
        }
    }
}
