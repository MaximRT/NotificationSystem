using Microsoft.AspNetCore.Mvc;

namespace NotificationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {

        [HttpGet("emailNotify")]
        public async Task<IActionResult> TestSendMessage()
        {

            return Ok();
        }
    }
}
