using MimeKit;

namespace NotificationSystem.Services
{
    public class EmailNotificationService(IMessageSender messageSender) : INotification
    {
        private readonly IMessageSender _messageSender = messageSender;

        public async Task Send(string recipient, string header, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Отправитель", "ponkratenkovm@internet.ru"));
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = header;
            message.Body = new TextPart("plain") { Text = body };

            await _messageSender.SendMessageAsync(message);
        }
    } 
}
