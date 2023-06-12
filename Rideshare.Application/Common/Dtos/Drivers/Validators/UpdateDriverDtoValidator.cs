using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Drivers.Validators
{
    public class UpdateDriverDtoValidator : AbstractValidator<UpdateDriverDto>
    {
        public UpdateDriverDtoValidator()
        {
            RuleFor(driver => driver.Id).GreaterThan(0).WithMessage("Id must be greater than zero");
            RuleFor(entity => entity.Name)
.NotEmpty().WithMessage("Name is required.");

            RuleFor(entity => entity.Rate)
                .GreaterThan(0).WithMessage("Rate must be greater than zero.").LessThan(11).WithMessage("Rate must be less than 11");

            RuleFor(entity => entity.Experience)
                .GreaterThanOrEqualTo(0).WithMessage("Experience must be a non-negative value.");

            RuleFor(entity => entity.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(entity => entity.LicenseNumber)
                .NotEmpty().WithMessage("License number is required.");

            RuleFor(entity => entity.License)
                .NotEmpty().WithMessage("License details are required.");


        }
    }
}
