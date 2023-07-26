// using FluentValidation;
// using Rideshare.Application.Features.Commuters.Queries;

// public class GetCommutersCountStatisticsQueryValidator : AbstractValidator<GetCommutersCountStatisticsQuery>
// {
//     public GetCommutersCountStatisticsQueryValidator()
//     {
//         RuleFor(x => x.Year)
//             .Must(BeValidYear)
//             .When(x => x.Year.HasValue)
//             .WithMessage("Invalid year. Year must be in range 2022 to the current year.");

//         RuleFor(x => x.Month)
//             .Must(BeValidMonth)
//             .When(x => x.Month.HasValue)
//             .WithMessage("Invalid month. Month must be in the range of 1 to 12.");


//         RuleFor(x => x.Year)
//             .Must(BeValidYearType)
//             .When(x => x.Year.HasValue)
//             .WithMessage("Year must be of type int.");

//         RuleFor(x => x.Month)
//             .Must(BeValidMonthType)
//             .When(x => x.Month.HasValue)
//             .WithMessage("Month must be of type int.");

//         // Allow null values for both year and month
//         // RuleFor(x => x)
//         //     .Must(BeValidYearAndMonthNullCombination)
//         //     .WithMessage("Both year and month cannot be null.");
//     }

//     private bool BeValidYear(int? year)
//     {
//         if (!year.HasValue)
//             return true;

//         int currentYear = DateTime.Now.Year;
//         return year.Value >= 2022 && year.Value <= currentYear;
//     }

//     private bool BeValidMonth(int? month)
//     {
//         if (!month.HasValue)
//             return true;

//         return month.Value >= 1 && month.Value <= 12;
//     }

//     private bool BeValidYearAndMonthCombination(int? year, int? month)
//     {
//         return year.HasValue && month.HasValue;
//     }

//     private bool BeValidYearType(int? year)
//     {
//         return year.HasValue && year.Value.GetType() == typeof(int);
//     }

//     private bool BeValidMonthType(int? month)
//     {
//         return month.HasValue && month.Value.GetType() == typeof(int);
//     }

//     private bool BeValidYearAndMonthNullCombination(GetCommutersCountStatisticsQuery query)
//     {
//         return !(query.Year == null && query.Month == null);
//     }
// }
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

        RuleFor(x => x.Year)
            .Must(BeValidYearType)
            .When(x => x.Year.HasValue)
            .WithMessage("Year must be of type int.");

        RuleFor(x => x.Month)
            .Must(BeValidMonthType)
            .When(x => x.Month.HasValue)
            .WithMessage("Month must be of type int.");

        // Allow null values for both year and month
        RuleFor(x => x)
            .Must(BeValidYearAndMonthNullCombination)
            .WithMessage("Both year and month cannot be null.");
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

    private bool BeValidYearType(int? year)
    {
        return year.HasValue && year.Value.GetType() == typeof(int);
    }

    private bool BeValidMonthType(int? month)
    {
        return month.HasValue && month.Value.GetType() == typeof(int);
    }

    private bool BeValidYearAndMonthNullCombination(GetCommutersCountStatisticsQuery query)
    {
        // Both year and month can be null
        return true;
    }
}
