using FluentValidation;
using ChatUpdater.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUpdater.Infrastructure.Validators.Account
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email Address should not be empty.")
            .NotNull().WithMessage("Email Address should not be null.")
            .EmailAddress().WithMessage("Email address is invalid.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username should not be empty.")
                .NotNull().WithMessage("Username should not be null.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number should not be empty.")
                .NotNull().WithMessage("Phone Number should not be null.")
                .Length(10).WithMessage("Phone number should be of 10 digits");
        }

    }
}
