using System.Collections.Generic;
using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideOffers;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetRideOffersQuery: IRequest<BaseResponse<IReadOnlyList<RideOfferListDto>>>
{
    public string DriverID { get; set; }
}
