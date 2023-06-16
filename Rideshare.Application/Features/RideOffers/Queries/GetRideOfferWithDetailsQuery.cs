using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideOffers;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetRideOfferWithDetailsQuery: IRequest<BaseResponse<RideOfferDto>>
{
    public int RideOfferID { get; set; }
}
