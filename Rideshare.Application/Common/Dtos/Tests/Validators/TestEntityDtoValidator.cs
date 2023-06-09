using FluentValidation;

namespace Rideshare.Application.Common.Dtos.Tests.Validators;

public class TestEntityDtoValidator: AbstractValidator<TestEntityDto>
{
    public TestEntityDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotNull().WithMessage("{PropertyName} is required.")
            .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

        RuleFor(p => p.OtherField)
            .NotNull().WithMessage("{PropertyName} is required.")
            .NotEmpty().WithMessage("{PropertyName} cannot be empty.");
    }
}
