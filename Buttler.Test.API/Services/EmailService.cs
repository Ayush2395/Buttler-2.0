using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Buttler.Test.API.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public bool SendEmailAsync(string email, string callBackURL)
        {
            MailMessage message = new();
            message.From = new MailAddress("ak005355@gmail.com");
            message.To.Add(new MailAddress(email));
            message.Subject = "Verify your email";
            message.Body = callBackURL;
            message.IsBodyHtml = true;

            SmtpClient client = new(_settings.Server);
            client.Credentials = new NetworkCredential(_settings.UserName, _settings.Password);
            client.Port = _settings.Port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Send(message);
            return true;
        }
    }

    public class EmailSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
