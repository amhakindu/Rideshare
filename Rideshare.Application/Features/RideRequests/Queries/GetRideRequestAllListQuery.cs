using MediatR;
using Rideshare.Application.Common.Dtos.RideRequests;
using Rideshare.Application.Responses;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.RideRequests.Queries;

public class GetRideRequestAllListQuery :  IRequest<BaseResponse<Dictionary<string,object>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    }
