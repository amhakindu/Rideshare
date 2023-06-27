using System.Collections.Generic;
using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideOffers;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetRideOffersOfDriverQuery: IRequest<BaseResponse<Dictionary<string, object>>>
{
    public string UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
