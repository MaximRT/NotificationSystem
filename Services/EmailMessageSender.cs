using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NotificationSystem.Configs;

namespace NotificationSystem.Services
{
    public class EmailMessageSender(ISmtpSettingsProvider smtpSettingsProvider, ILogger<EmailMessageSender> logger) : IMessageSender
    {
        private readonly ISmtpSettingsProvider _smtpSettingsProvider = smtpSettingsProvider;
        private readonly ILogger _logger = logger;

        /// <inheritdoc/>
        public async Task SendMessageAsync(MimeMessage message, string providerName)
        {
            try
            {
                _logger.LogInformation($"Method:EmailMessageSender.SendMessageAsync, StartMessageSendingProcess...");

                var settings = _smtpSettingsProvider.GetSettings(providerName);
                using var client = new SmtpClient();

                SecureSocketOptions socketOptions = settings.UseSsl
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTls;

                await client.ConnectAsync(settings.Host, settings.Port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(settings.Username, settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Method:EmailMessageSender.SendMessageAsync, CompletionMessageSendingProcess...");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method:EmailMessageSender.SendMessageAsync,, Error when sending a message : {ex}");
            }
        }
    }
}
