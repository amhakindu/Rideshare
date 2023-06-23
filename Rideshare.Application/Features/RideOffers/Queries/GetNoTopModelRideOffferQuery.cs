using MediatR;
using Rideshare.Application.Common.Dtos.RideOffers;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideOffers.Queries
{
    public class GetNoTopModelRideOffferQuery: IRequest<BaseResponse<IReadOnlyList<ModelAndCountDto>>>
    {

    }
}
