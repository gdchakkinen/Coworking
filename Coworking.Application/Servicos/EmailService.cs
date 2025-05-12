using Coworking.Application.Interfaces;
using Coworking.Domain.Entidades;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace Coworking.Application.Servicos
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task EnviaEmailAsync(string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_smtpSettings.SenderName,
                                                    _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("destino", email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_smtpSettings.Server,
                                                _smtpSettings.Port,
                                                MailKit.Security.SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(_smtpSettings.Username,
                                                   _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
