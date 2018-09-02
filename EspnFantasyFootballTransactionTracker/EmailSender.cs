using EspnFantasyFootballTransactionTracker.Model;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EspnFantasyFootballTransactionTracker
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfig = emailConfiguration.Value;
        }

        public Task SendEmailAsync(string subject, string message)
        {
            if (string.IsNullOrEmpty(_emailConfig.AuthorEmailAddress) || string.IsNullOrEmpty(_emailConfig.AuthorEmailPassword))
            {
                throw new Exception($"{nameof(EmailConfiguration)} is missing required user settings. Check that " +
                    $"{nameof(_emailConfig.AuthorEmailAddress)}, and {nameof(_emailConfig.AuthorEmailPassword)} have values.");
            }

            Execute(subject, message).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string subject, string message)
        {

            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(_emailConfig.AuthorEmailAddress, _emailConfig.UserFullName),
                Subject = subject,
                Body = message,
                Priority = MailPriority.High,
            };

            foreach (var recipient in _emailConfig.RecipientList.Split(','))
            {
                mail.CC.Add(new MailAddress(recipient.Trim()));
            }

            using (SmtpClient smtp = new SmtpClient(_emailConfig.SmtpDomain, _emailConfig.SmtpPort))
            {
                smtp.Credentials = new NetworkCredential(_emailConfig.AuthorEmailAddress, _emailConfig.AuthorEmailPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
