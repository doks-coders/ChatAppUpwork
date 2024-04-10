﻿using ChatUpdater.Models.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUpdater.Infrastructure.Validators.Message
{
    public class MessageRequestValidator : AbstractValidator<MessageRequest>
    {
        public MessageRequestValidator() 
        {
            RuleFor(x => x.Content)
           .NotEmpty().WithMessage("Message should not be empty.")
           .NotNull().WithMessage("Message should not be null.");

            RuleFor(x => x.RecieverId)
                .NotEmpty().WithMessage("Reciever Id should not be empty.")
                .NotNull().WithMessage("Reciever Id should not be null.");
        }
    }
}
