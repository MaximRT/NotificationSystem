using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationSystem.Configs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace NotificationSystem.Services
{
    public class EmailQueueConsumer(IServiceProvider serviceProvider,
                                    ILogger<EmailQueueConsumer> logger, IOptions<RabbitMQSettings> settings) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private IConnection _connection;
        //Проверить обязательно ли указывать дженерик тип
        private readonly ILogger<EmailQueueConsumer> _logger = logger;
        private IChannel _channel = null;
        private const string QueueName = "email_queue";
        private readonly RabbitMQSettings _settings = settings.Value; 

        private async Task InitializeRabbitMQ()
        {
            _connection = await CreateConnectionAsync();

            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken = default)
        {
            await InitializeRabbitMQ();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += ProcessMessageAsync;

            await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            await Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea)
        {
            using var scope = _serviceProvider.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailNotificationService>();

            try
            {
                var emailMessage = DeserializeMimeMessage(ea.Body.ToArray());
                await emailSender.SendEmailAsync
                    ("Outlook",
                     emailMessage.To.FirstOrDefault()?.ToString(), 
                     emailMessage.Subject, 
                     emailMessage.TextBody, 
                     "plain");

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);

                _logger.LogInformation("Email processed successfully. MessageId: {MessageId}", emailMessage.MessageId);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "MIME message format error");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, "Temporary SMTP error. Message requeued");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing email message");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        }

        private MimeMessage DeserializeMimeMessage(byte[] body)
        {
            using var memoryStream = new MemoryStream(body);
            return MimeMessage.Load(memoryStream);
        }

        private async Task<IConnection> CreateConnectionAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                VirtualHost = _settings.VirtualHost,
                UserName = _settings.Username,
                Password = _settings.Password,
                Port = _settings.Port
            };

            return await factory.CreateConnectionAsync();
        }
    }
}
