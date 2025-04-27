namespace NotificationSystem.Services
{
    public interface IMessageBrokerService
    {
        // <summary>
        /// Оптравка сообщения в очередь брокера сообщений
        /// </summary>
        /// <param name="message"> Текст соообщения </param>
        /// <param name="serviceName"> Название сервиса в конфиге c данными брокера </param>
        /// <returns></returns>
        Task SendMessageAsync(string message);
    }
}
