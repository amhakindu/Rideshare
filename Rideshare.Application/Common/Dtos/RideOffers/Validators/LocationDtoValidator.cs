using FluentValidation;

namespace Rideshare.Application.Common.Dtos.RideOffers.Validators;

public class LocationDtoValidator: AbstractValidator<LocationDto>
{
    public LocationDtoValidator()
    {
        RuleFor(point => point.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Invalid {PropertyName} coordinate");

        RuleFor(point => point.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Invalid {PropertyName} coordinate");   
    }
}
