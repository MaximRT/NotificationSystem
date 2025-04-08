using MimeKit;

namespace NotificationSystem.Services
{
    public interface IMessageSender
    {
        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message"> Объект сообщения </param>
        /// <returns></returns>
        Task SendMessageAsync(MimeMessage message);
    }
}
