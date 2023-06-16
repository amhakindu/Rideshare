using FluentValidation;
using Rideshare.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Feedbacks.Validators
{
    public class CreateFeedbackValidator : AbstractValidator<CreateFeedbackDto>
    {
        public CreateFeedbackValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(p => p.Title)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is Required!")
                .MaximumLength(500).WithMessage("{PropertyName} should not more than 500 words");
            RuleFor(p=>p.Content)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is Required!")
                .MaximumLength(500).WithMessage("{PropertyName} should not more than 500 words");
            RuleFor(p => p.Rating)
                .NotEmpty().WithMessage("{PropertyName} is Required!");
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is Required!");
        }
    }
}
