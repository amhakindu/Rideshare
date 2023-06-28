using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideOffers.Commands;

public class CancelRideOfferCommand: IRequest<BaseResponse<Unit>>
{
    public int RideOfferId { get; set; }
}
