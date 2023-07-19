using FluentValidation;
using Microsoft.AspNetCore.Http;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Common.Dtos.Vehicles.Validators;
public class CreateVehicleDtoValidator : AbstractValidator<CreateVehicleDto>
{
    public CreateVehicleDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(vehicle => vehicle.PlateNumber)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");
        RuleFor(vehicle => vehicle.NumberOfSeats)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");
        RuleFor(vehicle => vehicle.Model)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");
        RuleFor(vehicle => vehicle.Libre)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .Must((IFormFile pdf) =>
            {
                var extension = Path.GetExtension(pdf.FileName);
                return extension != null && extension.ToLower() == ".pdf";
            }).WithMessage("{PropertyName} must be a pdf");
        RuleFor(vehicle => vehicle.DriverId)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .MustAsync(async (int driverId, CancellationToken token) =>
            {
                return await unitOfWork.DriverRepository.Exists(driverId);
            }).WithMessage("The given {PropertyName} must exist in our database");
    }
}
