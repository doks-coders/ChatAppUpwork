using ChatUpdater.Models.Requests;
using FluentValidation;

namespace ChatUpdater.Infrastructure.Validators.User
{
    public class UpdateUserRequestValidation : AbstractValidator<UpdateUserInformationRequest>
    {
        public UpdateUserRequestValidation()
        {
            RuleFor(x => x.UserName)
           .NotEmpty().WithMessage("Message should not be empty.")
           .NotNull().WithMessage("Message should not be null.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Reciever Id should not be empty.")
                .NotNull().WithMessage("Reciever Id should not be null.")
                .MinimumLength(10);

        }
    }
}
