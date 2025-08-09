using Microsoft.AspNetCore.Mvc;
using Monolithic.Shared.Logging;
using Moq;

namespace Monolithic.Test.Shared.Logging;

/// <summary>
/// ApiLoggingActionFilter 的單元測試
/// </summary>
public class ApiLoggingActionFilterTests
{
    private readonly Mock<IAppLogger<ApiLoggingActionFilter>> _mockLogger;
    private readonly ApiLoggingActionFilter _filter;

    public ApiLoggingActionFilterTests()
    {
        _mockLogger = new Mock<IAppLogger<ApiLoggingActionFilter>>();
        _filter = new ApiLoggingActionFilter(_mockLogger.Object);
    }

    // 測試用的假 Controller
    private class TestController : ControllerBase { }
}
