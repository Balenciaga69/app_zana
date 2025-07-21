using BasicApp.Chat.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace BasicApp.Chat.Controllers
{
    // Handles GET requests and returns a greeting message with the provided name
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly ILogger<ExampleController> _logger;

        private readonly IHubContext<ChatHub> _hubContext;

        public ExampleController(ILogger<ExampleController> logger, IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        // Returns a greeting message for the given name
        [HttpGet("Hey/{name}")]
        public GetResult Get(string name)
        {
            IPAddress clientIpAddress = HttpContext.Connection.RemoteIpAddress;
            _logger.LogInformation($"接收到來自 IP: {clientIpAddress?.ToString()} 的請求");
            _logger.LogInformation("Hi!, {Name}", name);
            return new($"Hey!, {name}");
        }

        // Represents the result of a GET request with a default number.
        public readonly record struct GetResult(string Name, int Number = 65535);

        // Sends a notification message to all connected clients.
        // This method can be called via a POST request to notify all clients.
        [HttpGet("notify/{message}")]
        public async Task<IActionResult> NotifyAll(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Server", message);
            return Ok();
        }
    }
}
