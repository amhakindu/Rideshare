using FluentValidation;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers.Validators;

public class RideOffersListFilterDtoValidator: AbstractValidator<RideOffersListFilterDto>
{
    public RideOffersListFilterDtoValidator()
    {
        When(dto => dto.DriverName != null, ()=>{
            RuleFor(dto => dto.DriverName)
                .NotNull().WithMessage("{PropertyName} cannot be empty");
        });

        When(dto => dto.PhoneNumber != null, ()=>{
            RuleFor(dto => dto.PhoneNumber)
                .NotNull().WithMessage("{PropertyName} cannot be empty");
        });

        When(dto => dto.MinCost != null, ()=>{
            RuleFor(dto => dto.MinCost)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be greater than -1");
        });

        When(dto => dto.Status != null, ()=>{
            RuleFor(dto => dto.Status)
                .Must((status) => Enum.IsDefined(typeof(Status), status))
                .WithMessage("{PropertyName} must be WAITING, ONROUTE, COMPLETED or CANCELLED");
        });
    }
}
