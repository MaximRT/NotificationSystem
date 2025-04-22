namespace NotificationSystem.Services
{
    public interface IEmailNotificationService
    {
        /// <summary>
        /// Отправка сообщения по электронной почте.
        /// </summary>
        /// <param name="providerName"> Имя заголовка настроек провайдера из конфига </param>
        /// <param name="recipient"> Электронная почта получателя </param>
        /// <param name="header"> Заголовок письма </param>
        /// <param name="body"> Содержание письма </param>
        /// <param name="typeContent"> Формат содержимого письма: plain или html </param>
        /// <returns> Асинхронная операция отправки письма </returns>
        Task SendEmailAsync(string providerName, string recipient, string header, string body, string typeContent);
    }
}
