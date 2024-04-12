using ChatUpdater.Models.Requests;
using FluentValidation;

namespace ChatUpdater.Infrastructure.Validators.Group
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupChatRequest>
    {
        public CreateGroupValidator()
        {
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Name should not be empty.")
           .NotNull().WithMessage("Name should not be null.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description should not be empty.")
                .NotNull().WithMessage("Description should not be null.");
        }
    }
}
