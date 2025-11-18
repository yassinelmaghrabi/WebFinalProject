using MailKit.Net.Smtp;
using MimeKit;

namespace Api.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public MailKitEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Clinic System", emailSettings["Username"]));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                emailSettings["Host"],
                int.Parse(emailSettings["Port"]),
                bool.Parse(emailSettings["UseSsl"])
            );

            await client.AuthenticateAsync(
                emailSettings["Username"],
                emailSettings["Password"]
            );

            await client.SendAsync(email);
            await client.DisconnectAsync(true);
        }
    }
}

