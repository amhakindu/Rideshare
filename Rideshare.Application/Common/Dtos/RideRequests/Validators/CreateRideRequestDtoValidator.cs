using FluentValidation;
using Rideshare.Application.Common.Dtos.Common.Validators;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class CreateRideRequestDtoValidator : AbstractValidator<CreateRideRequestDto>
{
    

    public CreateRideRequestDtoValidator()
    {
        
        RuleFor(p => p.Origin)
            .NotNull().WithMessage("{PropertyName} can not null")
            .NotEqual(p => p.Destination).WithMessage(("{PropertyName} can not be equal to {ComparisonValue}"))
            .SetValidator(new LocationDtoValidator());

        RuleFor(p => p.Destination)
            .NotNull().WithMessage("{PropertyName} can not null")
            .SetValidator(new LocationDtoValidator());
        
        RuleFor(dto => dto.Origin)
            .NotEqual(dto => dto.Destination).WithMessage("Origin cannot be the same as the destination");
    }
}
