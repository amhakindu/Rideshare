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

namespace Rideshare.Application.Common.Dtos.Security.Validators
{
    public class UserCreationDtoValidators : AbstractValidator<UserCreationDto>
    {
        public UserCreationDtoValidators()
        {


            RuleFor(entity => entity.Age)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("Age must be a non-negative value.");


            RuleFor(entity => entity.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Matches(@"^\+251\d{9}$")
            .WithMessage("PhoneNumber should start with '+251' and be followed by 9 digits.");
            ;

          
            RuleFor(entity => entity.FullName)
                .NotEmpty().WithMessage("FullName is required.");
           

           
            RuleFor(entity => entity.Profilepicture)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .Must(BeValidImage)
            .When(entity => entity.Profilepicture != null)
            .WithMessage("Profile picture must be an image file.");

        }
        private bool BeValidImage(IFormFile? file)
        {
            if (file == null)
                return true;

           
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            return file.ContentType != null && allowedContentTypes.Contains(file.ContentType.ToLower())
                || file.FileName != null && allowedImageExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
        }
    }
}
