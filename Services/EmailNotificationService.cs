using MimeKit;
using NotificationSystem.Configs;

namespace NotificationSystem.Services
{
    public class EmailNotificationService(IMessageSender messageSender, ISmtpSettingsProvider smtpSettingsProvider,
                                          ILogger<EmailNotificationService> logger) : IEmailNotificationService
    {
        private readonly IMessageSender _messageSender = messageSender;
        private readonly ISmtpSettingsProvider _smtpSettingsProvider = smtpSettingsProvider;
        private readonly ILogger _logger = logger;

        /// <inheritdoc/>
        public async Task SendEmailAsync(string providerName, string recipient, string header, string body, string typeContent)
        {
            try
            {
                _logger.LogInformation($"Method:EmailNotificationService.SendEmailAsync, StartingMessageFormation...");
                
                var settings = _smtpSettingsProvider.GetSettings(providerName);
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(settings.FromName, settings.Username));
                message.To.Add(new MailboxAddress("", recipient));
                message.Subject = header;
                message.Body = new TextPart(typeContent) { Text = body };

                _logger.LogInformation($"Method:EmailNotificationService.SendEmailAsync, CompletionMessageFormation...");

                await _messageSender.SendMessageAsync(message, providerName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method:EmailNotificationService.SendEmailAsync, - {ex}");
            }
        }
    } 
}
