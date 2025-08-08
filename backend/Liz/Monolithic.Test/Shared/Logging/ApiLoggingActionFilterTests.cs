using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monolithic.Shared.Logging;
using Moq;
using System.Diagnostics;

namespace Monolithic.Test.Shared.Logging;

/// <summary>
/// ApiLoggingActionFilter 的單元測試 - TDD 範例
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

    /// <summary>
    /// 測試 OnActionExecuting 方法是否正確記錄請求資訊。
    /// </summary>
    [Fact]
    public void OnActionExecuting_ShouldLogRequestInformation()
    {
        // Arrange
        var context = CreateActionExecutingContext();

        // Act
        _filter.OnActionExecuting(context);

        // Assert
        _mockLogger.Verify(
            x => x.LogInfo(
                It.Is<string>(msg => msg.Contains("[API請求]") && msg.Contains("TestController")),
                It.IsAny<object>(),
                It.IsAny<string>()
            ),
            Times.Once
        );
    }

    /// <summary>
    /// 測試 OnActionExecuted 方法在沒有例外的情況下是否正確記錄成功回應。
    /// </summary>
    [Fact]
    public void OnActionExecuted_WhenNoException_ShouldLogSuccessResponse()
    {
        // Arrange
        var executingContext = CreateActionExecutingContext();
        var executedContext = CreateActionExecutedContext(statusCode: 200);

        // Act
        _filter.OnActionExecuting(executingContext);
        _filter.OnActionExecuted(executedContext);

        // Assert
        _mockLogger.Verify(
            x => x.LogInfo(
                It.Is<string>(msg => msg.Contains("[API回應]") && msg.Contains("Status: 200")),
                It.IsAny<object>(),
                It.IsAny<string>()
            ),
            Times.Once
        );
    }

    /// <summary>
    /// 測試 OnActionExecuted 方法在發生例外的情況下是否正確記錄錯誤。
    /// </summary>
    [Fact]
    public void OnActionExecuted_WhenException_ShouldLogError()
    {
        // Arrange
        var executingContext = CreateActionExecutingContext();
        var exception = new InvalidOperationException("Test exception");
        var executedContext = CreateActionExecutedContext(exception: exception);

        // Act
        _filter.OnActionExecuting(executingContext);
        _filter.OnActionExecuted(executedContext);

        // Assert
        _mockLogger.Verify(
            x => x.LogError(
                It.Is<string>(msg => msg.Contains("[API錯誤]")),
                It.Is<Exception>(ex => ex == exception),
                It.IsAny<object>(),
                It.IsAny<string>()
            ),
            Times.Once
        );
    }

    /// <summary>
    /// 測試 OnActionExecuted 方法是否正確記錄執行時間。
    /// </summary>
    [Fact]
    public void OnActionExecuted_ShouldLogExecutionDuration()
    {
        // Arrange
        var executingContext = CreateActionExecutingContext();
        var executedContext = CreateActionExecutedContext();

        // Act
        _filter.OnActionExecuting(executingContext);
        Thread.Sleep(10); // 確保有測量到時間
        _filter.OnActionExecuted(executedContext);

        // Assert
        _mockLogger.Verify(
            x => x.LogInfo(
                It.Is<string>(msg => msg.Contains("Duration:") && msg.Contains("ms")),
                It.IsAny<object>(),
                It.IsAny<string>()
            ),
            Times.Once
        );
    }

    /// <summary>
    /// 測試 OnActionExecuting 方法是否正確記錄查詢參數。
    /// </summary>
    [Fact]
    public void OnActionExecuting_WithQueryParameters_ShouldLogQueryParams()
    {
        // Arrange
        var context = CreateActionExecutingContext();
        context.HttpContext.Request.QueryString = new QueryString("?param1=value1&param2=value2");

        // Act
        _filter.OnActionExecuting(context);

        // Assert
        _mockLogger.Verify(
            x => x.LogInfo(
                It.IsAny<string>(),
                It.Is<object>(data => data.ToString()!.Contains("QueryParams")),
                It.IsAny<string>()
            ),
            Times.Once
        );
    }

    private ActionExecutingContext CreateActionExecutingContext()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "test-trace-id";

        var actionContext = new ActionContext(
            httpContext,
            new Microsoft.AspNetCore.Routing.RouteData(),
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
        );

        var controller = new TestController();
        var filters = new List<IFilterMetadata>();
        var actionArguments = new Dictionary<string, object?> { { "testParam", "testValue" } };

        return new ActionExecutingContext(actionContext, filters, actionArguments, controller);
    }

    private ActionExecutedContext CreateActionExecutedContext(int statusCode = 200, Exception? exception = null)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Response.StatusCode = statusCode;
        httpContext.TraceIdentifier = "test-trace-id";

        var actionContext = new ActionContext(
            httpContext,
            new Microsoft.AspNetCore.Routing.RouteData(),
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
        );

        var controller = new TestController();
        var filters = new List<IFilterMetadata>();

        return new ActionExecutedContext(actionContext, filters, controller)
        {
            Exception = exception
        };
    }

    // 測試用的假 Controller
    private class TestController : ControllerBase
    {
    }
}
