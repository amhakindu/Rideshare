using System.Data;
using FluentValidation;

namespace Rideshare.Application.Common.Dtos.RideOffers.Validators;

public class CreateRideOfferDtoValidator : AbstractValidator<CreateRideOfferDto>
{
    public static List<string> driverIDs { get; } 
        = new List<string> {"ASDF-1234-GHJK-5678", "QWER-1234-TYUI-5678", "ZXCV-1234-BNMM-5678"};
    public static List<int> vehicleIDs { get; } 
        = new List<int>{1, 2, 3, 4};

    public CreateRideOfferDtoValidator()
    {
        RuleFor(dto => dto.DriverID)
            .NotEmpty().WithMessage("Driver ID is required")
            .Must((string driverID) => {
                return driverIDs.Exists(o => o == driverID);
            });

        RuleFor(dto => dto.VehicleID)
            .NotEmpty().WithMessage("Vehicle ID is required")
            .Must((int vehicleID) => {
                return vehicleIDs.Exists(o => o == vehicleID);
            });

        RuleFor(dto => dto.CurrentLocation)
            .NotNull().WithMessage("Current location is required")
            .SetValidator(new LocationDtoValidator());

        RuleFor(dto => dto.Destination)
            .NotNull().WithMessage("Destination is required")
            .SetValidator(new LocationDtoValidator());

        RuleFor(dto => dto.CurrentLocation)
            .NotEqual(dto => dto.Destination)
            .WithMessage("{PropertyName} cannot have the same value as {ComparisonName}");
    }
}