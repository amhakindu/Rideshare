using FluentValidation;
using Rideshare.Application.Contracts.Identity;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class RideRequestDtoValidator : AbstractValidator<RideRequestDto>
{
    
    public RideRequestDtoValidator()
    {
        
        Include(new IRideRequestDtoValidator());
    }
}
