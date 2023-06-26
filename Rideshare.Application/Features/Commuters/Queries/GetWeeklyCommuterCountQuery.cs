using MediatR;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.Commuters.Queries
{
    public class GetWeeklyCommuterCountQuery : IRequest<BaseResponse<WeeklyCommuterCountDto>>
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
