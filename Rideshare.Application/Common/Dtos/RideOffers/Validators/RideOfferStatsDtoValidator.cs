using FluentValidation;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers.Validators;

public class RideOfferStatsDtoValidator: AbstractValidator<RideOfferStatsDto>
{
    public RideOfferStatsDtoValidator()
    {
        When(dto => dto.Year != null , ()=>{
            RuleFor(dto => dto.Year)
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("{PropetyName} must be current or past year");
        });

        When(dto => dto.Month != null, ()=>{
            RuleFor(dto => dto.Month)
                .LessThanOrEqualTo(12).WithMessage("{PropertyName} must be less than {ComparisonValue}")
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be greater than 0");
        });
    }
}
