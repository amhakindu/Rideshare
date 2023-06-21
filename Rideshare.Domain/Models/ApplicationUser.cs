using Microsoft.AspNetCore.Identity;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Models;

public class ApplicationUser : IdentityUser
{
  
    public int Age { get; set; }
    public string FullName { get; set; } = string.Empty;

    public string ProfilePicture { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

}
