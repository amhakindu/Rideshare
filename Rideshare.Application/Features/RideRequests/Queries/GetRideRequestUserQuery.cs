using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestUserQuery : IRequest<BaseResponse<Dictionary<string,object>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? UserId { get; set; }
}
