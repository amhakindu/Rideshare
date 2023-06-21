using FluentValidation;
using Rideshare.Application.Contracts.Identity;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class IRideRequestDtoValidator : AbstractValidator<IRideRequestDto>
{
    // private readonly IUserRepository _userRepository;

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
        .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");

        // RuleFor(p => p.UserId)
        // .MustAsync( async (id,token) => {
        //     var user = await _userRepository.FindByIdAsync(id);
        //     return user != null;
        // }).WithMessage("{PropertyName}is not found");

       


    }
   
}
