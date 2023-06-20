using System.Data;
using FluentValidation;
using Rideshare.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rideshare.Application.Contracts.Identity;

namespace Rideshare.Application.Common.Dtos.Drivers.Validators
{
    public class CreateDriverDtoValidator : AbstractValidator<CreateDriverDto>
    {
        public CreateDriverDtoValidator()
        {


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
