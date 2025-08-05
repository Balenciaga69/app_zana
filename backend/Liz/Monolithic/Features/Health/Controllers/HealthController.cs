using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Monolithic.Features.Health.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// 基本健康檢查
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// 詳細健康檢查，包含所有依賴服務
        /// </summary>
        [HttpGet("detailed")]
        public async Task<IActionResult> GetDetailed()
        {
            var healthReport = await _healthCheckService.CheckHealthAsync();
            var response = new
            {
                status = healthReport.Status.ToString(),
                totalDuration = healthReport.TotalDuration.TotalMilliseconds,
                timestamp = DateTime.UtcNow,
                entries = healthReport.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    duration = entry.Value.Duration.TotalMilliseconds,
                    description = entry.Value.Description,
                    exception = entry.Value.Exception?.Message,
                    tags = entry.Value.Tags,
                }),
            };

            return CreateHealthResponse(healthReport.Status, response);
        }

        /// <summary>
        /// 檢查資料庫連線
        /// </summary>
        [HttpGet("database")]
        public async Task<IActionResult> GetDatabase()
        {
            return await GetHealthByTag("database", "databases");
        }

        /// <summary>
        /// 檢查快取服務連線
        /// </summary>
        [HttpGet("cache")]
        public async Task<IActionResult> GetCache()
        {
            return await GetHealthByTag("cache", "caches");
        }

        /// <summary>
        /// 檢查訊息佇列連線
        /// </summary>
        [HttpGet("messaging")]
        public async Task<IActionResult> GetMessaging()
        {
            return await GetHealthByTag("messaging", "messaging");
        }

        /// <summary>
        /// 通用的標籤健康檢查方法
        /// </summary>
        private async Task<IActionResult> GetHealthByTag(string tag, string propertyName)
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(check => check.Tags.Contains(tag));
            var taggedChecks = healthReport.Entries.Where(e => e.Value.Tags.Contains(tag));

            var response = CreateTaggedHealthResponse(healthReport.Status, taggedChecks, propertyName);
            return CreateHealthResponse(healthReport.Status, response);
        }

        /// <summary>
        /// 建立標籤化健康檢查回應
        /// </summary>
        private object CreateTaggedHealthResponse(
            HealthStatus status,
            IEnumerable<KeyValuePair<string, HealthReportEntry>> checks,
            string propertyName
        )
        {
            var properties = new Dictionary<string, object>
            {
                ["status"] = status.ToString(),
                ["timestamp"] = DateTime.UtcNow,
                [propertyName] = checks.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    duration = entry.Value.Duration.TotalMilliseconds,
                    description = entry.Value.Description,
                }),
            };

            return properties;
        }

        /// <summary>
        /// 建立統一的健康檢查 HTTP 回應
        /// </summary>
        private IActionResult CreateHealthResponse(HealthStatus status, object response)
        {
            var statusCode = status == HealthStatus.Healthy ? 200 : 503;
            return StatusCode(statusCode, response);
        }
    }
}
