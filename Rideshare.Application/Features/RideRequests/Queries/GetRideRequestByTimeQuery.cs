using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestByTimeQuery : IRequest<BaseResponse<Dictionary<int,int>>>
{
    public string? type { get; set; }
    public int year { get; set; }
    public int month { get; set; }
}
