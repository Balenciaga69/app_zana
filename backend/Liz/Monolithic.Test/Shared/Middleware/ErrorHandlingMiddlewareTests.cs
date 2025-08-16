using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Monolithic.Shared.Common;
using Monolithic.Shared.Middleware;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Monolithic.Test.Shared.Middleware;

public class ErrorHandlingMiddlewareTests
{
    /*
     * TODO: Not Implemented:
     * 當沒有例外時，應該正常呼叫下一個 middleware。
     * 回傳內容應包含 TraceId。
     * 應正確處理 message 與 stack trace。
     * 應正確處理自訂訊息與預設訊息的情境。
     * 應正確處理多種常見例外（如 UnauthorizedAccessException, ArgumentException, KeyNotFoundException 等）。
     */

    #region Helper Methods

    /// 建立測試用的 HttpContext，包含 MemoryStream 作為 ResponseBody
    private static (DefaultHttpContext context, MemoryStream responseBody) CreateTestHttpContext(
        string traceId = "test-trace-123"
    )
    {
        var context = new DefaultHttpContext(); // DefaultHttpContext 用於模擬 HTTP 請求上下文
        var responseBody = new MemoryStream(); // MemoryStream 用於模擬 HTTP 回應內容
        context.Response.Body = responseBody;
        context.TraceIdentifier = traceId;
        return (context, responseBody);
    }

    /// 從 ResponseBody 讀取並反序列化 ApiResponse
    private static async Task<ApiResponse<object>?> ReadApiResponseFromBodyAsync(MemoryStream responseBody)
    {
        // Seek 是為了重置 MemoryStream 的位置
        responseBody.Seek(0, SeekOrigin.Begin);
        // ReadToEndAsync: 讀取 MemoryStream 的所有內容
        var json = await new StreamReader(responseBody, Encoding.UTF8).ReadToEndAsync();
        return JsonSerializer.Deserialize<ApiResponse<object>>(
            json,
            // PropertyNameCaseInsensitive: 忽略 JSON 屬性名稱的大小寫
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
    }

    #endregion

    /// <summary>
    /// 測試當 middleware 捕捉到例外時，應該正確記錄 log 並回傳統一格式的錯誤訊息
    /// </summary>
    [Fact]
    public async Task Invoke_WhenExceptionThrown_ShouldLogErrorAndReturnUnifiedErrorResponse()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var mockLogger = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var (context, responseBody) = CreateTestHttpContext();

        // 模擬下一個 middleware 直接丟出例外
        RequestDelegate next = (ctx) => throw exception;
        var middleware = new ErrorHandlingMiddleware(next, mockLogger.Object);

        // Act
        await middleware.Invoke(context);

        // Assert
        // 檢查 log 是否有被呼叫
        mockLogger.Verify(
            x =>
                x.Log(
                    LogLevel.Error,
                    // IsAny: EventId 允許任何事件 ID
                    It.IsAny<EventId>(),
                    // Is: 允許任何類型的訊息
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unhandled exception")),
                    // exception: 允許任何類型的例外
                    exception,
                    // IsAny: 允許任何格式化函式
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
            Times.Once
        );

        // 檢查 response
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.Should().Be("application/json");

        var apiResponse = await ReadApiResponseFromBodyAsync(responseBody);

        apiResponse.Should().NotBeNull();
        apiResponse!.Success.Should().BeFalse();
        apiResponse.Code.Should().Be(ErrorCode.InternalServerError.ToString());
        apiResponse.TraceId.Should().Be("test-trace-123");
        apiResponse.Message.Should().NotBeNullOrEmpty();
        apiResponse.Errors.Should().NotBeNull();
    }

    /// <summary>
    /// 測試 middleware 根據不同的 Exception 類型，回傳正確的 HTTP 狀態碼與錯誤碼
    /// </summary>
    [Theory]
    [InlineData(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized, ErrorCode.AuthRequired)]
    [InlineData(typeof(ArgumentException), HttpStatusCode.BadRequest, ErrorCode.InvalidInput)]
    [InlineData(typeof(KeyNotFoundException), HttpStatusCode.NotFound, ErrorCode.ResourceNotFound)]
    [InlineData(typeof(Exception), HttpStatusCode.InternalServerError, ErrorCode.InternalServerError)]
    public async Task Invoke_WhenSpecificExceptionThrown_ShouldReturnCorrectStatusCodeAndErrorCode(
        Type exceptionType,
        HttpStatusCode expectedStatusCode,
        ErrorCode expectedErrorCode
    )
    {
        // Arrange
        // Activator.CreateInstance 用於動態建立指定類型的例外實例
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test exception")!;
        var mockLogger = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var (context, responseBody) = CreateTestHttpContext();

        // 模擬下一個 middleware 直接丟出指定的例外
        RequestDelegate next = (ctx) => throw exception;
        var middleware = new ErrorHandlingMiddleware(next, mockLogger.Object);

        // Act
        await middleware.Invoke(context);

        // Assert
        context.Response.StatusCode.Should().Be((int)expectedStatusCode);
        context.Response.ContentType.Should().Be("application/json");

        var apiResponse = await ReadApiResponseFromBodyAsync(responseBody);

        apiResponse.Should().NotBeNull();
        apiResponse!.Success.Should().BeFalse();
        apiResponse.Code.Should().Be(expectedErrorCode.ToString());
        apiResponse.TraceId.Should().Be("test-trace-123");
        apiResponse.Message.Should().NotBeNullOrEmpty();
        apiResponse.Errors.Should().NotBeNull();
    }
}
