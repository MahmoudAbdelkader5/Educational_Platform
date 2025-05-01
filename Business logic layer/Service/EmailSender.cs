using Business_logic_layer.interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;


namespace Business_logic_layer.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true
            };

            var message = new MailMessage(_emailSettings.SenderEmail, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);
        }
    }
}
