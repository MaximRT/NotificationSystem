using Microsoft.Extensions.Options;

namespace NotificationSystem.Configs
{
    public class SmtpSettingsProvider(IOptions<SmtpSettingsOptions> options) : ISmtpSettingsProvider
    {
        private readonly SmtpSettingsOptions _options = options.Value;
        public SmtpProviderSettings GetSettings(string providerName)
        {
            if (_options.Providers.TryGetValue(providerName.ToLower(), out var settings))
            {
                return settings;
            }

            throw new ArgumentException($"SMTP settings for provider '{providerName}' not found.");
        }
    }
}
