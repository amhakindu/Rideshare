

using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.Security;
public class UserDto
{

    public string Id { get; set; }
	public List<RoleDto> Roles { get; } = new();
	public string FullName { get; set; } = string.Empty;
	public string PhoneNumber { get; set; } = string.Empty;
	public int Age { get; set; }
	public string StatusByLogin {get;set;}

}
