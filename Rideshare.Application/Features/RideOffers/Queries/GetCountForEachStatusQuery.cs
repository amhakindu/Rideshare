using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.RideOffers.Queries;

public class GetCountForEachStatusQuery: IRequest<BaseResponse<Dictionary<string, int>>>
{
}
