using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Monolithic.Shared.Common;
using Monolithic.Shared.Middleware;
using Moq;
using System.Net;
using System.Text.Json;

namespace Monolithic.Test.Shared.Middleware;

public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<RequestDelegate> _mockNext;
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _mockLogger;
    private readonly ErrorHandlingMiddleware _middleware; private readonly DefaultHttpContext _httpContext;

    public ErrorHandlingMiddlewareTests()
    {
        _mockNext = new Mock<RequestDelegate>();
        _mockLogger = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _middleware = new ErrorHandlingMiddleware(_mockNext.Object, _mockLogger.Object);
        _httpContext = new DefaultHttpContext();

        // 設置可寫入的 Response Body
        _httpContext.Response.Body = new MemoryStream();
    }

    [Fact]
    public async Task Invoke_WhenNoException_ShouldCallNext()
    {
        // Arrange
        _mockNext.Setup(x => x(_httpContext)).Returns(Task.CompletedTask);

        // Act
        await _middleware.Invoke(_httpContext);

        // Assert
        _mockNext.Verify(x => x(_httpContext), Times.Once);
        _httpContext.Response.StatusCode.Should().Be(200); // 預設值
    }
    [Fact]
    public async Task Invoke_WhenExceptionOccurs_ShouldReturnApiResponseFailFormat()
    {
        // Arrange
        var testException = new InvalidOperationException("Test error message");
        var traceId = "test-trace-456";
        _httpContext.TraceIdentifier = traceId;

        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act
        await _middleware.Invoke(_httpContext);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        _httpContext.Response.ContentType.Should().Be("application/json");

        // 驗證回應內容
        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        responseContent.Should().NotBeEmpty();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        apiResponse.Should().NotBeNull();
        apiResponse!.Success.Should().BeFalse();
        apiResponse.Code.Should().Be("InternalServerError");
        apiResponse.Message.Should().Be("伺服器發生未預期錯誤，請稍後再試。");
        apiResponse.TraceId.Should().Be(traceId);
        apiResponse.Errors.Should().NotBeNull();
    }

    [Fact]
    public async Task Invoke_WhenExceptionOccurs_ShouldLogError()
    {
        // Arrange
        var testException = new ArgumentNullException("testParam", "Test null reference");
        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act
        await _middleware.Invoke(_httpContext);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unhandled exception")),
                testException,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
    [Fact]
    public async Task Invoke_WhenExceptionOccurs_ShouldIncludeStackTraceInErrors()
    {
        // Arrange
        var testException = new DivideByZeroException("Division by zero occurred");
        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act
        await _middleware.Invoke(_httpContext);

        // Assert
        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        responseContent.Should().NotBeEmpty();
        responseContent.Should().Contain("Division by zero occurred");
        responseContent.Should().Contain("StackTrace");
    }

    [Fact]
    public async Task Invoke_WhenExceptionOccurs_ShouldSetCorrectTimestamp()
    {
        // Arrange
        var testException = new TimeoutException("Operation timed out");
        var beforeInvoke = DateTime.UtcNow;
        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act
        await _middleware.Invoke(_httpContext);

        // Assert
        var afterInvoke = DateTime.UtcNow;

        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        responseContent.Should().NotBeEmpty();

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        apiResponse!.Timestamp.Should().BeOnOrAfter(beforeInvoke).And.BeOnOrBefore(afterInvoke);
    }

    [Fact]
    public async Task Invoke_WhenExceptionWithCustomMessage_ShouldIncludeInErrors()
    {
        // Arrange
        var customMessage = "Custom business logic error occurred";
        var testException = new ApplicationException(customMessage);
        _mockNext.Setup(x => x(_httpContext)).ThrowsAsync(testException);

        // Act
        await _middleware.Invoke(_httpContext);

        // Assert
        _httpContext.Response.Body.Position = 0;
        var responseContent = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        responseContent.Should().NotBeEmpty();
        responseContent.Should().Contain(customMessage);
    }
}
