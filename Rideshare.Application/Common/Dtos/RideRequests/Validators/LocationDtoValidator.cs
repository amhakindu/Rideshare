using FluentValidation;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class LocationDtoValidator : AbstractValidator<LocationDto>
{
    public LocationDtoValidator()
    {
        RuleFor(point => point.longitude)
            .InclusiveBetween(-180, 180).WithMessage("Invalid {PropertyName} coordinate");
        RuleFor(point => point.latitude)
            .InclusiveBetween(-90, 90).WithMessage("Invalid {PropertyName} coordinate");
   
    }
}
