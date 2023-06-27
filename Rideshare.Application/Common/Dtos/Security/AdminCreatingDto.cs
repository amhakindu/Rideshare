

using Microsoft.AspNetCore.Http;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.Security;
public class AdminCreationDto
{
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Age { get; set; }

    public IFormFile? Profilepicture { get; set; }


}