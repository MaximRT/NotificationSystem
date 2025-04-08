using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace NotificationSystem.Services
{
    public class EmailMessageSender : IMessageSender
    {
        public async Task SendMessageAsync(MimeMessage message)
        {
            using var client = new SmtpClient();

            await client.ConnectAsync("smtp.mail.ru", 465, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync("ponkratenkovm@internet.ru", "au5r5iW9MVL6CdRQgghb");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
