using MediatR;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Feedbacks.Queries
{
    public class GetFeedbackListByUserIdQuery: IRequest<BaseResponse<IReadOnlyList<FeedbackDto>>>
    {
        public string UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
