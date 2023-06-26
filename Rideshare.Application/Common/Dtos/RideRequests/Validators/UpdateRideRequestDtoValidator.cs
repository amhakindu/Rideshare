using FluentValidation;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class UpdateRideRequestDtoValidator : AbstractValidator<UpdateRideRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
     public UpdateRideRequestDtoValidator(IUnitOfWork unitOfWork)
    {
        
        _unitOfWork = unitOfWork;

        // RuleFor(p => p.Id)
        // .MustAsync(async (id,token) => {
        //     return await _unitOfWork.RideRequestRepository.Exists(id);
            
        // }).WithMessage("{PropertyName} does not exist"); 

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
