using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Application.Features.Common;
using Rideshare.Application.Common.Dtos.Feedbacks;

namespace Rideshare.Application.Features.Feedbacks.Queries;
public class GetFeedbackListQuery: PaginatedQuery, IRequest<PaginatedResponse<FeedbackDto>>
{
}
