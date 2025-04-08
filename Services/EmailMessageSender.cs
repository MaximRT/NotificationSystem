using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace NotificationSystem.Services
{
    public class EmailMessageSender(IOptions<EmailSettings> setting) : IMessageSender
    {
        private readonly EmailSettings _setting = setting.Value;

        public async Task SendMessageAsync(MimeMessage message)
        {
            using var client = new SmtpClient();

            SecureSocketOptions socketOptions = _setting.UseSsl
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTls;

            await client.ConnectAsync(_setting.SmtpServer, _setting.Port, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_setting.FromEmail, _setting.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
