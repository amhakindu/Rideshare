using FluentValidation;

namespace Rideshare.Application.Common.Dtos.Rates.Validators
{
	public class CreateRateDtoValidator: AbstractValidator<CreateRateDto>
	{   
		public CreateRateDtoValidator()
		{
			
			RuleFor(p => p.Rate)
				.NotNull().WithMessage("{PropertyName} is required.")
				.NotEmpty().WithMessage("{PropertyName} cannot be empty.")
				.Must(rate => double.TryParse(rate.ToString(), out _)).WithMessage("{PropertyName} must be a valid number.")
				.InclusiveBetween(1, 10).WithMessage("{PropertyName} must be between 1 and 10.");;

			RuleFor(p => p.Description)
				.NotNull().WithMessage("{PropertyName} is required.")
				.NotEmpty().WithMessage("{PropertyName} cannot be empty.")
				.Length(5, 400).WithMessage("{PropertyName} must be between 5 and 400 characters long.")
				.Matches("^[A-Za-z0-9 ,.-]+$").WithMessage("{PropertyName} must only contain letters, numbers, spaces, commas, dots, or hyphens.");;
			
			RuleFor(p => p.RaterId)
				.NotEmpty().WithMessage("{PropertyName} cannot be empty.");

			RuleFor(p => p.DriverId)
				.NotEmpty().WithMessage("{PropertyName} cannot be empty.");
				
		}
    }
}