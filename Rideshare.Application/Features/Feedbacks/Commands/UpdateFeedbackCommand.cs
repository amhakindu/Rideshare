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
    public class UpdateFeedbackCommand: IRequest<BaseResponse<Unit>>
    {
        public UpdateFeedbackDto feedbackDto { get; set; }
    }
}
