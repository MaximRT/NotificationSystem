using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotificationSystem.Configs;
using NotificationSystem.Services;
using Org.BouncyCastle.Cms;
using System.Text;
using System.Text.Json;

namespace NotificationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController(IEmailNotificationService emailNotificationService, ISmtpSettingsProvider smtpSettingsProvider,
                                        IMessageBrokerService messageBrokerService, ILogger<NotificationController> logger) : ControllerBase
    {
        private readonly IEmailNotificationService _emailNotificationService = emailNotificationService;
        private readonly ISmtpSettingsProvider _smtpSettingsProvider = smtpSettingsProvider;
        private readonly IMessageBrokerService _messageBrokerService = messageBrokerService;
        private readonly ILogger _logger = logger;

        [HttpGet("emailNotify")]
        public async Task<IActionResult> TestSendMessage()
        {
            try
            {
                var settings = _smtpSettingsProvider.GetSettings("Outlook");
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(settings.FromName, settings.Username));
                message.To.Add(new MailboxAddress("", "Ponkratenkov.Maksim@vitta.ru"));
                message.Subject = "Внимание!";
                message.Body = new TextPart("plain") { Text = "TestMessage" };

                string serialized;

                using (var stream = new MemoryStream())
                {
                    message.WriteTo(stream);
                    serialized = Encoding.UTF8.GetString(stream.ToArray());
                }

                await _messageBrokerService.SendMessageAsync(serialized);

                //_logger.LogInformation($"Method:NotificationController.TestSendMessage, message: StartSendingMessegeFromController...");

                //await _emailNotificationService.SendEmailAsync("mailru", "maks.ponkratenkov@mail.ru", "Тестовое сообщение", "Tекс сообщения", "plain");

                //_logger.LogInformation($"Method:NotificationController.TestSendMessage, message: CompletionSendingMessegeFromController...");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method:NotificationController.TestSendMessage, message: ErrorInProcessSendingMessegeFromController..., error: {ex}");
                return BadRequest();
            } 
        }
    }
}
