
using FluentValidation;
using Rideshare.Application.Features.Commuters.Queries;

public class GetCommutersCountStatisticsQueryValidator : AbstractValidator<GetCommutersCountStatisticsQuery>
{
    public GetCommutersCountStatisticsQueryValidator()
    {
        RuleFor(x => x.Year)
            .Must(BeValidYear)
            .When(x => x.Year.HasValue)
            .WithMessage("Invalid year. Year must be in range 2022 to the current year.");

        RuleFor(x => x.Month)
            .Must(BeValidMonth)
            .When(x => x.Month.HasValue)
            .WithMessage("Invalid month. Month must be in the range of 1 to 12.");

        RuleFor(x => x)
            .Must(BeValidYearAndMonthCombination)
            .WithMessage("If month is specified, year must also be specified.");


    }

    private bool BeValidYear(int? year)
    {
        if (!year.HasValue)
            return true;

        int currentYear = DateTime.Now.Year;
        return year.Value >= 2022 && year.Value <= currentYear;
    }

    private bool BeValidMonth(int? month)
    {
        if (!month.HasValue)
            return true;

        return month.Value >= 1 && month.Value <= 12;
    }

    private bool BeValidYearAndMonthCombination(GetCommutersCountStatisticsQuery query)
    {
        // If month is specified, year must also be specified
        return !query.Month.HasValue || query.Year.HasValue;
    }

}
