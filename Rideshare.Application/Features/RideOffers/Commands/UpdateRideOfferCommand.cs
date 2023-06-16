using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideOffers;

namespace Rideshare.Application.Features.RideOffers.Commands;

public class UpdateRideOfferCommand: IRequest<BaseResponse<Unit>>
{
    public UpdateRideOfferDto RideOfferDto { get; set; }  
}

