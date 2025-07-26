using BasicApp.Chat.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BasicApp.Chat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotifyController : ControllerBase
    {
        private readonly ILogger<NotifyController> _logger;

        private readonly IHubContext<ChatHub> _hubContext;

        public NotifyController(ILogger<NotifyController> logger, IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpGet("notify/{message}")]
        public async Task<IActionResult> NotifyAll(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Server", message);
            return Ok();
        }
    }
}
