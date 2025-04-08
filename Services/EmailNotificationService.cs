using Microsoft.Extensions.Options;
using MimeKit;

namespace NotificationSystem.Services
{
    public class EmailNotificationService(IMessageSender messageSender, IOptions<EmailSettings> settings) : INotification
    {
        private readonly IMessageSender _messageSender = messageSender;
        private readonly EmailSettings _settings = settings.Value;

        public async Task Send(string recipient, string header, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = header;
            message.Body = new TextPart("plain") { Text = body };

            await _messageSender.SendMessageAsync(message);
        }
    } 
}
