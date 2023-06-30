using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestAllStatusStatsticsQuery : IRequest<BaseResponse<Dictionary<string,int>>>
{
}
