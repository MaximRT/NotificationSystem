using Microsoft.AspNetCore.Mvc;
using NotificationSystem.Services;

namespace NotificationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController(IEmailNotificationService emailNotificationService, ILogger<NotificationController> logger) : ControllerBase
    {
        private readonly IEmailNotificationService _emailNotificationService = emailNotificationService;
        private readonly ILogger _logger = logger;

        [HttpGet("emailNotify")]
        public async Task<IActionResult> TestSendMessage()
        {
            try
            {
                _logger.LogInformation($"Method:NotificationController.TestSendMessage, message: StartSendingMessegeFromController...");

                await _emailNotificationService.SendEmailAsync("mailru", "maks.ponkratenkov@mail.ru", "Тестовое сообщение", "Tекс сообщения", "plain");

                _logger.LogInformation($"Method:NotificationController.TestSendMessage, message: CompletionSendingMessegeFromController...");
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
