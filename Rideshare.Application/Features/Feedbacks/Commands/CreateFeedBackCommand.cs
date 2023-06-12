using MediatR;
using Rideshare.Application.Common.Dtos.Feedbacks;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Feedbacks.Commands
{
    public class CreateFeedBackCommand: IRequest<BaseResponse<Unit>>
    {
        public CreateFeedbackDto  feedbackDto { get; set; }
    }
}
