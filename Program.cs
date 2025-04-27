using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NotificationSystem.Configs;
using NotificationSystem.Services;
using RabbitMQ.Client;
using Serilog;

namespace NotificationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Serilog.ILogger serilogLogger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(serilogLogger);

            builder.Services.AddControllers();
            builder.Services.Configure<SmtpSettingsOptions>(
            builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.AddSingleton<ISmtpSettingsProvider, SmtpSettingsProvider>();
            builder.Services.AddSingleton<IMessageBrokerService, RabbitMQService>();
            builder.Services.AddSingleton<IEmailNotificationService, EmailNotificationService>();
            builder.Services.AddSingleton<IMessageSender, EmailMessageSender>();

            builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMqConfiguration"));
            builder.Services.AddHostedService<EmailQueueConsumer>();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
