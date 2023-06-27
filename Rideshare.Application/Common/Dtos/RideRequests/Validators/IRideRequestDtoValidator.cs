using FluentValidation;
using Rideshare.Application.Contracts.Identity;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class IRideRequestDtoValidator : AbstractValidator<IRideRequestDto>
{
    public IRideRequestDtoValidator( )
    {
        
        // RuleFor(p => p.Origin)
        //         .NotNull().WithMessage("{PropertyName} can not null")
        //         .NotEqual(p => p.Destination).WithMessage(("{PropertyName} can not be equal to {ComparisonValue}"))
        //         .SetValidator(new LocationDtoValidator());

        // RuleFor(p => p.Destination)
        //         .NotNull().WithMessage("{PropertyName} can not null")
        //         .SetValidator(new LocationDtoValidator());

        // RuleFor(p => p.NumberOfSeats)
        //         .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");
    }   
}
