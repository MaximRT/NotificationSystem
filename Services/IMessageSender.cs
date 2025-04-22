using MimeKit;

namespace NotificationSystem.Services
{
    public interface IMessageSender
    {
        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message"> Объект сообщения </param>
        /// <param name="providerName"> Имя провайдера с настройками из конфига </param>
        /// <returns></returns>
        Task SendMessageAsync(MimeMessage message, string providerName);
    }
}
