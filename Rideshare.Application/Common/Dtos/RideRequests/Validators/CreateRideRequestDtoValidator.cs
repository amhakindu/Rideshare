using FluentValidation;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class CreateRideRequestDtoValidator : AbstractValidator<CreateRideRequestDto>
{

    public CreateRideRequestDtoValidator()
    {
        Include(new IRideRequestDtoValidator());

    }
}
