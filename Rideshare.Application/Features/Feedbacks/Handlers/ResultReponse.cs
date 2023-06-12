using MediatR;
using Rideshare.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.Feedbacks.Handlers
{
    public class ResultReponse
    {
        public BaseResponse<Unit> ValidationErrorReponse(List<string> errors)
        {
            return new BaseResponse<Unit>
            {
                Success = false,
                Errors = errors,
                Message = "Feedback not created!",
            };
        }
        public BaseResponse<Unit> SuccessReponse()
        {
            return new BaseResponse<Unit>
            {
                Success = true,
                Message = "Feedback created successfully."
            };
        }

        public BaseResponse<Unit> FailureReponse()
        {
            return new BaseResponse<Unit>
            {
                Success = false,
                Errors = new List<string> { "Feedback save Failed!" },
                Message = "Feedback not created!"
            };
        }
    }
}
