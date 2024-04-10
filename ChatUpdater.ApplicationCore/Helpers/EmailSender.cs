using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ChatUpdater.ApplicationCore.Helpers
{
    public class EmailSender:IEmailSender
    {
        public string SendGridSecret;
        public EmailSender(IConfiguration config)
        {
            SendGridSecret = config.GetValue<string>("SendGridKey");
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SendGridClient Client = new SendGridClient(SendGridSecret);

            EmailAddress fromSender = new EmailAddress(email, "Title");
            EmailAddress toSender = new EmailAddress(email);
            SendGridMessage? message = MailHelper.CreateSingleEmail(fromSender, toSender, subject, null, htmlMessage);
            return Client.SendEmailAsync(message);

        }
    }
    public interface IEmailSender
    {
        public Task SendEmailAsync(string email, string subject,string htmlMessage);
    }
}
