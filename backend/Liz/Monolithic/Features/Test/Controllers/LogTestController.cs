using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogTestController : ControllerBase
{
    private readonly IAppLogger<LogTestController> _appLogger;

    public LogTestController(IAppLogger<LogTestController> appLogger)
    {
        _appLogger = appLogger;
    }

    [HttpGet("test-serilog")]
    public IActionResult TestSerilogConfiguration()
    {
        // 測試一般資訊日誌
        _appLogger.LogInfo("測試 Serilog 配置", new {
            TestData = "這是測試資料",
            Timestamp = DateTime.UtcNow
        });

        // 測試警告日誌
        _appLogger.LogWarn("這是警告測試", new {
            WarningLevel = "Medium",
            Component = "LogTestController"
        });

        // 測試敏感資料遮罩
        var sensitiveData = new
        {
            Username = "testuser",
            Password = "secret123", // 應該被遮罩
            ApiKey = "abc123def456", // 應該被遮罩
            Token = "jwt.token.here", // 應該被遮罩
            PublicInfo = "這是公開資訊"
        };

        _appLogger.LogInfo("測試敏感資料遮罩", sensitiveData);

        // 測試過長資料遮罩
        var longData = new
        {
            ShortText = "簡短文字",
            Url = "https://very-long-url-that-should-be-masked.example.com/with/many/path/segments", // 應該被遮罩
            Content = "這是一段很長的內容，應該會被標記為過長資料而被遮罩處理" // 應該被遮罩
        };

        _appLogger.LogInfo("測試過長資料遮罩", longData);

        return Ok(new {
            Message = "Serilog 配置測試完成",
            LogsLocation = "請檢查 logs/ 資料夾和 Console 輸出",
            Features = new[]
            {
                "結構化日誌",
                "敏感資料遮罩 (Password, Token, ApiKey)",
                "過長資料遮罩 (Url, Content)",
                "環境資訊擴充 (ThreadId, MachineName, EnvironmentName)",
                "檔案輪替 (每日)",
                "Console 與檔案雙重輸出"
            }
        });
    }

    [HttpGet("test-error")]
    public IActionResult TestErrorLogging()
    {
        try
        {
            // 故意拋出異常來測試錯誤日誌
            throw new InvalidOperationException("這是測試用的例外");
        }
        catch (Exception ex)
        {
            _appLogger.LogError("測試錯誤日誌記錄", ex, new {
                ErrorContext = "LogTestController.TestErrorLogging",
                UserId = "test-user-123"
            });

            return BadRequest(new {
                Message = "錯誤已記錄到日誌中",
                ErrorId = Activity.Current?.Id ?? "unknown"
            });
        }
    }
}
