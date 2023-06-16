using FluentValidation;
using NetTopologySuite.Geometries;
using Rideshare.Application.Contracts.Persistence;

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
                .IsInEnum().WithMessage("Invalid status")
                .When(dto => dto.Status != null);
        });
    }
}