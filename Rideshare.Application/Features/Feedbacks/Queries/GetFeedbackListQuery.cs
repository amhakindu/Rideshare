using MediatR;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Feedbacks.Queries
{
    public class GetFeedbackListQuery: IRequest<BaseResponse<List<FeedbackDto>>>
    {
    }
}
