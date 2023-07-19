using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestByTimeQuery : TimeseriesQuery, IRequest<BaseResponse<Dictionary<int,int>>>
{ 
}
