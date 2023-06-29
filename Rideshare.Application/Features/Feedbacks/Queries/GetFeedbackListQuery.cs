using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Common.Dtos.Pagination;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Feedbacks.Queries
{
    public class GetFeedbackListQuery: IRequest<BaseResponse<PaginatedResponseDto<FeedbackDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
