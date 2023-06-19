using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Features.Userss;

namespace Rideshare.Infrastructure.Security;

 
     public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public UserAccessor(IHttpContextAccessor httpContextAccessor,IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public string GetUsername()
        {
             return  _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value;
            
            
        }
    }
 