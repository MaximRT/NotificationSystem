using Microsoft.Extensions.Options;
using MimeKit;

namespace NotificationSystem.Services
{
    public class EmailNotificationService(IMessageSender messageSender, IOptions<EmailSettings> settings) : IEmailNotification
    {
        private readonly IMessageSender _messageSender = messageSender;
        private readonly EmailSettings _settings = settings.Value;

        /// <inheritdoc/>
        public async Task SendEmailAsync(string recipient, string header, string body, string typeContent)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
                message.To.Add(new MailboxAddress("", recipient));
                message.Subject = header;
                message.Body = new TextPart(typeContent) { Text = body };

                await _messageSender.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            
        }
    } 
}
