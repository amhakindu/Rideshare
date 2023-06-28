

using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.Security;
public class UserDriverDto
{

    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Age { get; set; }
    public string StatusByLogin { get; set; }
    public int DriverId { get; set; }
    public string UserId { get; set; }
    public string? ProfilePicture { get; set; } = string.Empty;

}