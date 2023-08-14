using FluentValidation;
using Microsoft.AspNetCore.Http;
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
            

            RuleFor(driver => driver.Experience)
                .GreaterThanOrEqualTo(0).WithMessage("Experience must be a non-negative value.");

            RuleFor(driver => driver.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(driver => driver.LicenseNumber)
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


        }
    }
}
