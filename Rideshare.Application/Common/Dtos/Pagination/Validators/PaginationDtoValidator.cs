using System;
using FluentValidation;

namespace Rideshare.Application.Common.Dtos.Pagination.Validators
{
    public class PaginationDtoValidator: AbstractValidator<PaginationDto>
    {
        
        public PaginationDtoValidator() 
        {

            RuleFor(p => p.PageSize).GreaterThan(0).WithMessage("{PropertyName} Must Be Greater Than 0");
            RuleFor(p => p.PageNumber).GreaterThan(0).WithMessage("{PropertyName} Must Be Greater Than 0");

            
        }
    }
}