using FluentValidation;
using Rideshare.Application.Common.Dtos.Common.Validators;

namespace Rideshare.Application.Common.Dtos.RideOffers.Validators;

public class CreateRideOfferDtoValidator : AbstractValidator<CreateRideOfferDto>
{

    public CreateRideOfferDtoValidator()
    {
        RuleFor(dto => dto.VehicleID)
            .NotEmpty().WithMessage("Vehicle ID is required");

        RuleFor(dto => dto.CurrentLocation)
            .NotNull().WithMessage("Current location is required")
            .SetValidator(new LocationDtoValidator());

        RuleFor(dto => dto.Destination)
            .NotNull().WithMessage("Destination is required")
            .SetValidator(new LocationDtoValidator());

        RuleFor(dto => dto.CurrentLocation)
            .NotEqual(dto => dto.Destination)
            .WithMessage("{PropertyName} cannot have the same coordinate as Destination");
    }
}