using FluentValidation;

namespace Rideshare.Application.Common.Dtos.Commuter.Validators
{
    public class WeeklyCommuterCountRequestValidator : AbstractValidator<(int year, int month)>
    {
        public WeeklyCommuterCountRequestValidator()
        {
            RuleFor(request => request.year)
                .NotEmpty()
                .GreaterThanOrEqualTo(2022)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Invalid year. Year must be between 2022 and the current year.");

            RuleFor(request => request.month)
                .NotEmpty()
                .InclusiveBetween(1, 12)
                .WithMessage("Invalid month. Month must be between 1 and 12.");
        }
    }
}
