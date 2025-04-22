namespace NotificationSystem.Configs
{
    public interface ISmtpSettingsProvider
    {
        /// <summary>
        /// Получить настройки для конкретного провайдера
        /// </summary>
        /// <param name="providerName"> Имя провайдера </param>
        /// <returns> Объект с данными для отправки </returns>
        SmtpProviderSettings GetSettings(string providerName);
    }
}
