

using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.Security;
public class AdminUpdatingDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Age { get; set; }

}