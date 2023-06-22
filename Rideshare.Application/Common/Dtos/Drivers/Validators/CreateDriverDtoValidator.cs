using System.Data;
using FluentValidation;
using Rideshare.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rideshare.Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;

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
            RuleFor(driver => driver.License)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .Must((IFormFile image) =>
            {
                var extension = Path.GetExtension(image.FileName);
                HashSet<string> validTypes = new HashSet<string>() { ".png", ".jiff", ".img", ".jpg","jfif" };
                
                return extension != null && validTypes.Contains(extension.ToLower()) ;
            }).WithMessage("{PropertyName} must be an image");

            RuleFor(entity => entity.License)
                .NotEmpty().WithMessage("License details are required.");

        }
    }
}
