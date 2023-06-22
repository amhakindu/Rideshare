

using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.Security;
public class UserDto
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Age { get; set; }

}