using FluentValidation;
using Rideshare.Application.Common.Dtos.Tests;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");
        RuleFor(vehicle => vehicle.DriverId)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .MustAsync(async (int driverId, CancellationToken token) =>
            {
                return await unitOfWork.DriverRepository.Exists(driverId);
            }).WithMessage("The given {PropertyName} must exist in our database");
    }
}
