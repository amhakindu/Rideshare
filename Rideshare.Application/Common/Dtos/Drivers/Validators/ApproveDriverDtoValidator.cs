using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Dtos.Drivers.Validators
{
    public class ApproveDriverDtoValidator : AbstractValidator<ApproveDriverDto>
    {
        public ApproveDriverDtoValidator() {

            RuleFor(driver => driver.Id).NotEmpty().GreaterThan(0).WithMessage("Id Must Be Greater Than zero");
            RuleFor(driver => driver.Verified).NotEmpty().WithMessage("{PropertyName} Shall Not be empty").Must((verified) => verified is bool).WithMessage("Value Must Be Boolean");
        }
    }
}
