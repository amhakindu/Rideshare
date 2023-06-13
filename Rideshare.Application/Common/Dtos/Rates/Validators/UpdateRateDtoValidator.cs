using FluentValidation;

namespace Rideshare.Application.Common.Dtos.Rates.Validators
{
	public class UpdateRateDtoValidator : AbstractValidator<UpdateRateDto>
	{
		public UpdateRateDtoValidator()
		{
			RuleFor(p => p.Rate)
				.NotNull().WithMessage("{PropertyName} is required.")
				.InclusiveBetween(1, 10).WithMessage("{PropertyName} must be between 1 and 10.");

			RuleFor(p => p.Description)
				.NotNull().WithMessage("{PropertyName} is required.")
				.NotEmpty().WithMessage("{PropertyName} cannot be empty.")
				.Length(5, 400).WithMessage("{PropertyName} must be between 5 and 400 characters long.")
				.Matches("^[A-Za-z0-9 ,.-]+$").WithMessage("{PropertyName} must only contain letters, numbers, spaces, commas, dots, or hyphens.");
		}
	}
}