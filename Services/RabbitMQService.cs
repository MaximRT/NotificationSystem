
using NotificationSystem.Configs;
using RabbitMQ.Client;
using System.Text;

namespace NotificationSystem.Services
{
    public class RabbitMQService(IConfiguration configuration) : IMessageBrokerService
    {
        private readonly IConfiguration _configuration = configuration;
        private RabbitMQSettings _rabbitMQConfig;

        public async Task SendMessageAsync(string message)
        {
            _rabbitMQConfig = _configuration.GetSection($"RabbitMqConfiguration")?.Get<RabbitMQSettings>() ?? throw new NullReferenceException("Данные конфигурации для отправки не были найдены");

            var messageToSend = message ?? throw new NullReferenceException($"Отправляемое сообщение было null");
            var config = _rabbitMQConfig ?? throw new NullReferenceException($"Данные конфигурации для отправки не были найдены");
            var hostName = _rabbitMQConfig.HostName ?? throw new NullReferenceException($"В файле конфигурации не указан HostName");
            var virtualHost = _rabbitMQConfig.VirtualHost ?? throw new NullReferenceException($"В файле конфигурации не указан VirtualHost");
            var userName = _rabbitMQConfig.Username ?? throw new NullReferenceException($"В файле конфигурации не указан UserName");
            var password = _rabbitMQConfig.Password ?? throw new NullReferenceException($"В файле конфигурации не указан Password");
            var queue = _rabbitMQConfig.QueueName;
            var exchange = _rabbitMQConfig.ExchangeName;

            if (queue == null && exchange == null)
            {
                throw new NullReferenceException($"В файле конфигурации не указан Queue или Exchange");
            }

            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                VirtualHost = virtualHost,
                UserName = userName,
                Password = password,
            };

            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            var body = Encoding.UTF8.GetBytes(messageToSend);

            await channel.BasicPublishAsync(exchange: exchange ?? "",
                              routingKey: queue ?? "",
                              mandatory: true,
                              body: body);
        }
    }
}
