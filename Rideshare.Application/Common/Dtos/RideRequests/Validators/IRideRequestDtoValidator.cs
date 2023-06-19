using FluentValidation;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class IRideRequestDtoValidator : AbstractValidator<IRideRequestDto>
{

    public IRideRequestDtoValidator( )
    {
        RuleFor(p => p.Origin)
        .NotNull().WithMessage("{PropertyName} can not null")
        .NotEqual(p => p.Destination).WithMessage(("{PropertyName} can not be equal to {ComparisonValue}"))
        .SetValidator(new LocationDtoValidator());

        RuleFor(p => p.Destination)
        .NotNull().WithMessage("{PropertyName} can not null")
        .SetValidator(new LocationDtoValidator());
        
        RuleFor(p => p.CurrentFare)
        .GreaterThan(50).WithMessage("{PropertyName} must be greater than 50 birr");

        RuleFor(p => p.Status)
        .IsInEnum().WithMessage("{PropertyName} must be from the status enum");

        RuleFor(p => p.NumberOfSeats)
        .GreaterThan(0).WithMessage("{ProperyName} must be greater than zero");

       


    }
   
}
