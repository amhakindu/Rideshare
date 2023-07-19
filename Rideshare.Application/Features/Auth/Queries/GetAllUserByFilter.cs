using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Auth.Queries;

public class GetUsersByFilterQuery : IRequest<PaginatedResponse<UserDtoForAdmin>>
{
    public string PhoneNumber { get; set; }
    public string RoleName { get; set; }
    public string FullName { get; set; }
    public string Status { get; set; } // "ACTIVE" or "INACTIVE"
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
