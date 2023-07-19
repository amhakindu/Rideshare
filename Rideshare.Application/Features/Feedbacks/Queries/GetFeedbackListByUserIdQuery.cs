using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Common.Dtos.Feedbacks;

namespace Rideshare.Application.Features.Feedbacks.Queries
{
    public class GetFeedbackListByUserIdQuery: PaginatedQuery, IRequest<BaseResponse<IReadOnlyList<FeedbackDto>>>
    {
        public string UserId { get; set; }
    }
}
