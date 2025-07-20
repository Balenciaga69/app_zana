using BasicApp.Chat.Hubs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        [EnableCors("DenyAll")]
        public GetResult Get(string name)
        {
            _logger.LogInformation("Hi!, {Name}", name);
            return new($"Hey!, {name}");
        }

        // Represents the result of a GET request with a default number.
        public readonly record struct GetResult(string Name, int Number = 65535);

        // Sends a notification message to all connected clients.
        // This method can be called via a POST request to notify all clients.
        [HttpPost("notify/{message}")]
        [EnableCors("DenyAll")]
        [HttpPost("notify")]
        public async Task<IActionResult> NotifyAll([FromBody] string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Server", message);
            return Ok();
        }
    }
}
