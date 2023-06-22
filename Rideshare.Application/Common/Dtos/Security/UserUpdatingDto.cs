

using Microsoft.AspNetCore.Http;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.Security;
public class UserUpdatingDto
{
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public IFormFile? Profilepicture { get; set; }

}