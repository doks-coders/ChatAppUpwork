using ChatUpdater.Models.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUpdater.Infrastructure.Validators.Account
{
    public class SetPasswordRequestValidator : AbstractValidator<SetPasswordRequest>
    {
        public SetPasswordRequestValidator()
        {
            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password should not be empty.")
            .NotNull().WithMessage("Password should not be null.")
             .MinimumLength(6).WithMessage("Confirm Password should be have at least 6 characters");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password should not be empty.")
                .NotNull().WithMessage("Confirm Password should not be null.")
                .MinimumLength(6).WithMessage("Confirm Password should be have at least 6 characters");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User Id is missing")
                .NotNull().WithMessage("User Id should not be null");
        }
    }
}
