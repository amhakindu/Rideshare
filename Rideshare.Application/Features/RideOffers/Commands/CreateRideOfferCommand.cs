using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Common.Dtos.RideOffers;

namespace Rideshare.Application.Features.RideOffers.Commands;

public class CreateRideOfferCommand: IRequest<BaseResponse<int>>
{
    public CreateRideOfferDto RideOfferDto { get; set; }  
}

