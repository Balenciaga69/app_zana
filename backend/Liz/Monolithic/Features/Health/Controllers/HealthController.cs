using Microsoft.AspNetCore.Mvc;
using Monolithic.Shared.Common;

namespace Liz.Monolithic.Features.Health.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Healthy");
        }
    }
}
