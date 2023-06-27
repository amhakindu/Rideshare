

using Microsoft.AspNetCore.Http;
using Rideshare.Application.Common.Dtos.Drivers;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.Security;
public class DriverCreatingDto
{
    public RoleDto Roles { get; set; } = new();
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Age { get; set; }
    public CreateDriverDto DriverDto {get;set;} = new();
    public IFormFile? Profilepicture { get; set; }

}