using FluentValidation;
using Rideshare.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Feedbacks.Validators
{
    public class DeleteFeedbackValidator: AbstractValidator<int>
    {
        public DeleteFeedbackValidator(IUnitOfWork unitOfWork)
        {
            
            RuleFor(Id=> Id)
            .MustAsync(async (id, token) =>
                    await unitOfWork.FeedbackRepository.Exists(id)).WithMessage("feedback with given {PropertyName} Does not exist!");
        }
    }
}
