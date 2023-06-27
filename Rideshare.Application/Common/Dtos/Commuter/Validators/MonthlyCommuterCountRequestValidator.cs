using FluentValidation;

namespace Rideshare.Application.Common.Dtos.Commuter.Validators
{
    public class MonthlyCommuterCountRequestValidator : AbstractValidator<int>
    {
        public MonthlyCommuterCountRequestValidator()
        {
            RuleFor(year => year)
                .NotEmpty()
                .GreaterThanOrEqualTo(2022)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Invalid year. Year must be between 2022 and the current year.");
        }
    }
}
