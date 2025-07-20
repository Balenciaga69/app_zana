using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BasicApp.Chat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly ILogger<ExampleController> _logger;

        public ExampleController(ILogger<ExampleController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Hey/{name}")]
        [EnableCors("DenyAll")]
        public GetResult Get(string name)
        {
            _logger.LogInformation($"Hi!,{name}");
            return new($"Hey!,{name}");
        }

        public readonly record struct GetResult(string name, int number = 65535);
    }
}
