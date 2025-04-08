using MimeKit;

namespace NotificationSystem.Services
{
    public interface IMessageSender
    {
        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageAsync(MimeMessage message);
    }
}
