using FluentValidation;
using NetTopologySuite.Geometries;
using Rideshare.Application.Common.Dtos.Common.Validators;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Common.Dtos.RideOffers.Validators;


public class UpdateRideOfferDtoValidator : AbstractValidator<UpdateRideOfferDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRideOfferDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
        RuleFor(dto => dto.Id)
            .NotEmpty().WithMessage("{PropertyName} is required");

        When(dto => dto.VehicleID != null, ()=>{
            RuleFor(dto => dto.VehicleID)
                .NotEmpty().WithMessage("{PropertyName} is required");
        });

        When(dto => dto.CurrentLocation != null, ()=>{
            RuleFor(dto => dto.CurrentLocation)
                .SetValidator(new LocationDtoValidator());
        });

        When(dto => dto.Destination != null, ()=>{
            RuleFor(dto => dto.Destination)
                .SetValidator(new LocationDtoValidator());
        });

        When(dto => dto.Status != null, ()=>{
            RuleFor(dto => dto.Status)
                .Must((status) => Enum.IsDefined(typeof(Status), status))
                .WithMessage("{PropertyName} must be waiting, noroute, completed or cancelled");
        });
    }
}