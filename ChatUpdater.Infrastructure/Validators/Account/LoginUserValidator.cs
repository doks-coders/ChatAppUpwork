using ChatUpdater.Models.Requests;
using FluentValidation;

namespace ChatUpdater.Infrastructure.Validators.Account
{
    public class LoginUserValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email Address should not be empty.")
           .NotNull().WithMessage("Email Address should not be null.")
           .EmailAddress().WithMessage("Email address is invalid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password should not be empty.")
                .NotNull().WithMessage("Password should not be null.");

        }
    }
}
