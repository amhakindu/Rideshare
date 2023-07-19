using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestStatusStatsticsQuery : TimeseriesQuery, IRequest<BaseResponse<Dictionary<string,Dictionary<int,int>>>>
{
    public string option { get; set; }
}
