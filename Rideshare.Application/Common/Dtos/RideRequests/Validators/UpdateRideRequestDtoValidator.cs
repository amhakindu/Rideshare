using FluentValidation;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Common.Dtos.RideRequests.Validators;

public class UpdateRideRequestDtoValidator : AbstractValidator<UpdateRideRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
     public UpdateRideRequestDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        Include(new IRideRequestDtoValidator()); 

        RuleFor(p => p.Id)
        .MustAsync(async (id,token) => {
            var rideRequestExists = await _unitOfWork.RideRequestRepository.Exists(id);
            return rideRequestExists;
        }).WithMessage("{PropertyName} does not exist"); 
    }
}
