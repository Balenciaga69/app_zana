using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Monolithic.Shared.Common;
using Monolithic.Shared.Middleware;
using Moq;
using System.Net;
using System.Text.Json;

namespace Monolithic.Test.Shared.Middleware;

/// <summary>
/// 錯誤處理中介軟體的單元測試類別。
/// 測試中介軟體在不同情況下的行為，包括正常執行、拋出例外、記錄錯誤等。
/// </summary>
public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<RequestDelegate> _mockNext;
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _mockLogger;
    private readonly ErrorHandlingMiddleware _middleware;
    private readonly DefaultHttpContext _httpContext;

    /// <summary>
    /// 初始化測試所需的模擬物件和中介軟體實例。
    /// </summary>
    public ErrorHandlingMiddlewareTests()
    {
        _mockNext = new Mock<RequestDelegate>();
        _mockLogger = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _middleware = new ErrorHandlingMiddleware(_mockNext.Object, _mockLogger.Object);
        _httpContext = new DefaultHttpContext();

        // 設置可寫入的 Response Body，方便測試回應內容。
        _httpContext.Response.Body = new MemoryStream();
    }

    /// <summary>
    /// 測試當沒有例外發生時，中介軟體是否正確調用下一個委派。
    /// </summary>
    [Fact]
    public async Task Invoke_WhenNoException_ShouldCallNext()
    {
        // Arrange: 模擬下一個委派正常執行。
        _mockNext.Setup(x => x(_httpContext)).Returns(Task.CompletedTask);

        // Act: 執行中介軟體。
        await _middleware.Invoke(_httpContext);

        // Assert: 驗證下一個委派被正確調用，且回應狀態碼為預設值 200。
        _mockNext.Verify(x => x(_httpContext), Times.Once);
        _httpContext.Response.StatusCode.Should().Be(200);
    }

    /// <summary>
    /// 測試當發生例外時，中介軟體是否返回正確的 API 回應格式。
    /// </summary>
    [Fact]
    public async Task Invoke_WhenExceptionOccurs_ShouldReturnApiResponseFailFormat()
    {
        // Arrange: 模擬拋出例外，並設置 TraceIdentifier。
        var testException = new InvalidOperationException("Test error message");
        var traceId = "test-trace-456";
        _httpContext.TraceIdentifier = traceId;

        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act: 執行中介軟體。
        await _middleware.Invoke(_httpContext);

        // Assert: 驗證回應內容是否符合失敗的 API 回應格式。
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        _httpContext.Response.ContentType.Should().Be("application/json");

        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        responseContent.Should().NotBeEmpty();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(
            responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        apiResponse.Should().NotBeNull();
        apiResponse!.Success.Should().BeFalse();
        apiResponse.Code.Should().Be("InternalServerError");
        apiResponse.Message.Should().Be("伺服器發生未預期錯誤，請稍後再試。");
        apiResponse.TraceId.Should().Be(traceId);
        apiResponse.Errors.Should().NotBeNull();
    }

    /// <summary>
    /// 測試當發生例外時，中介軟體是否正確記錄錯誤。
    /// </summary>
    [Fact]
    public async Task Invoke_WhenExceptionOccurs_ShouldLogError()
    {
        // Arrange: 模擬拋出例外。
        var testException = new ArgumentNullException("testParam", "Test null reference");
        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act: 執行中介軟體。
        await _middleware.Invoke(_httpContext);

        // Assert: 驗證錯誤是否被正確記錄。
        _mockLogger.Verify(
            x =>
                x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unhandled exception")),
                    testException,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
            Times.Once
        );
    }

    /// <summary>
    /// 測試當發生例外時，回應內容是否包含例外的堆疊追蹤資訊。
    /// </summary>
    [Fact]
    public async Task Invoke_WhenExceptionOccurs_ShouldIncludeStackTraceInErrors()
    {
        // Arrange: 模擬拋出例外。
        var testException = new DivideByZeroException("Division by zero occurred");
        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act: 執行中介軟體。
        await _middleware.Invoke(_httpContext);

        // Assert: 驗證回應內容是否包含例外訊息及堆疊追蹤。
        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        responseContent.Should().NotBeEmpty();
        responseContent.Should().Contain("Division by zero occurred");
        responseContent.Should().Contain("StackTrace");
    }

    /// <summary>
    /// 測試當發生例外時，回應內容是否包含正確的時間戳。
    /// </summary>
    [Fact]
    public async Task Invoke_WhenExceptionOccurs_ShouldSetCorrectTimestamp()
    {
        // Arrange: 模擬拋出例外，並記錄執行前的時間。
        var testException = new TimeoutException("Operation timed out");
        var beforeInvoke = DateTime.UtcNow;
        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act: 執行中介軟體。
        await _middleware.Invoke(_httpContext);

        // Assert: 驗證回應內容的時間戳是否在執行前後的範圍內。
        var afterInvoke = DateTime.UtcNow;

        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        responseContent.Should().NotBeEmpty();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(
            responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        apiResponse!.Timestamp.Should().BeOnOrAfter(beforeInvoke).And.BeOnOrBefore(afterInvoke);
    }

    /// <summary>
    /// 測試當例外包含自訂訊息時，回應內容是否正確包含該訊息。
    /// </summary>
    [Fact]
    public async Task Invoke_WhenExceptionWithCustomMessage_ShouldIncludeInErrors()
    {
        // Arrange: 模擬拋出例外，並設置自訂訊息。
        var customMessage = "Custom business logic error occurred";
        var testException = new ApplicationException(customMessage);
        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act: 執行中介軟體。
        await _middleware.Invoke(_httpContext);

        // Assert: 驗證回應內容是否包含自訂訊息。
        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        responseContent.Should().NotBeEmpty();
        responseContent.Should().Contain(customMessage);
    }
}
