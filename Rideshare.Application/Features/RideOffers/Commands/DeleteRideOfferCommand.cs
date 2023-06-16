using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideOffers.Commands;

public class DeleteRideOfferCommand: IRequest<BaseResponse<Unit>>
{
    public int RideOfferID { get; set; }  
}

