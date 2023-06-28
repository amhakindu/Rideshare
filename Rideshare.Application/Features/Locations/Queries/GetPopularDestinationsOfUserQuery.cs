using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Locations.Queries;

public class GetPopularDestinationsOfUserQuery: IRequest<BaseResponse<IList<Dictionary<string, object>>>>
{
    public string UserId { get; set; }
    public int Limit { get; set; }
}
