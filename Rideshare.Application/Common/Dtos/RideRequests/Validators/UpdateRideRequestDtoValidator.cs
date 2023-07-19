using FluentValidation;
using Rideshare.Application.Common.Dtos.Common.Validators;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class UpdateRideRequestDtoValidator : AbstractValidator<UpdateRideRequestDto>
{   
     public UpdateRideRequestDtoValidator()
    {
        RuleFor(dto => dto.Origin)
            .NotNull().WithMessage("Current location is required")
            .SetValidator(new LocationDtoValidator());

        RuleFor(dto => dto.Destination)
            .NotNull().WithMessage("Destination is required")
            .SetValidator(new LocationDtoValidator());

        RuleFor(dto => dto.Origin)
            .NotEqual(dto => dto.Destination).WithMessage("Origin cannot be the same as the destination");
    }
}
