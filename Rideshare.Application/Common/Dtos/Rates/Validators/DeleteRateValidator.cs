using FluentValidation;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Common.Dtos.Rates.Validators
{
    public class DeleteRateValidator: AbstractValidator<int>
    {
        public DeleteRateValidator(IUnitOfWork unitOfWork)
        {
            
            RuleFor(Id => Id)
            .MustAsync(async (id, token) =>
                await unitOfWork.RateRepository.Exists(id)).WithMessage("Rate with ID {PropertyValue} Does not exist!");
        }
    }
}