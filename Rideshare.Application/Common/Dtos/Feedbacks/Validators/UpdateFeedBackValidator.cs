using FluentValidation;
using Rideshare.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Feedbacks.Validators
{
    public class UpdateFeedbackValidator: AbstractValidator<UpdateFeedbackDto>
    {
        public UpdateFeedbackValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(p => p.Title)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is Required!")
                .MaximumLength(500).WithMessage("{ProperyName} should not more than 500 words");
            RuleFor(p => p.Content)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is Required!")
                .MaximumLength(500).WithMessage("{ProperyName} should not more than 500 words");
        }
    }
}
