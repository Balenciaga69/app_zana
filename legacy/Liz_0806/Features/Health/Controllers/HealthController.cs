using MediatR;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Features.Health.Requests;

namespace Monolithic.Features.Health.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HealthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 基本健康檢查 (MediatR)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetHealthStatusQuery(false);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// 詳細健康檢查，包含所有依賴服務 (MediatR)
        /// </summary>
        [HttpGet("detailed")]
        public async Task<IActionResult> GetDetailed()
        {
            var query = new GetHealthStatusQuery(true);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// 檢查資料庫連線 (MediatR)
        /// </summary>
        [HttpGet("database")]
        public async Task<IActionResult> GetDatabase()
        {
            var query = new GetHealthStatusQuery(true) { Tag = "database", PropertyName = "databases" };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// 檢查快取服務連線 (MediatR)
        /// </summary>
        [HttpGet("cache")]
        public async Task<IActionResult> GetCache()
        {
            var query = new GetHealthStatusQuery(true) { Tag = "cache", PropertyName = "caches" };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// 檢查訊息佇列連線 (MediatR)
        /// </summary>
        [HttpGet("messaging")]
        public async Task<IActionResult> GetMessaging()
        {
            var query = new GetHealthStatusQuery(true) { Tag = "messaging", PropertyName = "messaging" };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
