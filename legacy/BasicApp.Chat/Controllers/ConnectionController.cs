using BasicApp.Chat.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasicApp.Chat.Controllers;

/// <summary>
/// 連線監控 API 控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConnectionController : ControllerBase
{
    private readonly IConnectionService _connectionService;
    private readonly ILogger<ConnectionController> _logger;

    public ConnectionController(IConnectionService connectionService, ILogger<ConnectionController> logger)
    {
        _connectionService = connectionService;
        _logger = logger;
    }
    /// <summary>
    /// 健康檢查端點
    /// </summary>
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Service = "BasicApp.Chat.ConnectionService"
        });
    }
}
