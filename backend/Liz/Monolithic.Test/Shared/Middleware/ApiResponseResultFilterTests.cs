using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monolithic.Shared.Common;
using Monolithic.Shared.Middleware;
using Moq;

namespace Monolithic.Test.Shared.Middleware;

/// <summary>
/// ApiResponseResultFilter 的單元測試類別，用於測試該過濾器的行為是否符合預期。
/// </summary>
public class ApiResponseResultFilterTests
{
    private readonly ApiResponseResultFilter _filter;

    /// <summary>
    /// 初始化測試類別，建立 ApiResponseResultFilter 實例。
    /// </summary>
    public ApiResponseResultFilterTests()
    {
        _filter = new ApiResponseResultFilter();
    }

    /// <summary>
    /// 建立 ResultExecutingContext 測試用的輔助方法。
    /// </summary>
    private ResultExecutingContext CreateResultExecutingContext(
        IActionResult result,
        string traceId = "test-trace"
    )
    {
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = traceId;

        var actionContext = new ActionContext(
            httpContext,
            new Microsoft.AspNetCore.Routing.RouteData(),
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
        );
        var filters = new List<IFilterMetadata>();

        return new ResultExecutingContext(actionContext, filters, result, new object());
    }

    /// <summary>
    /// 測試當 ObjectResult 的狀態碼為 200 時，是否正確包裝成 ApiResponse。
    /// </summary>
    [Fact]
    public async Task OnResultExecutionAsync_WhenObjectResultWith200Status_ShouldWrapInApiResponse()
    {
        // Arrange
        var testData = new { foo = "bar", id = 123 };
        var objectResult = new ObjectResult(testData) { StatusCode = 200 };
        var traceId = "test-trace-123";
        var context = CreateResultExecutingContext(objectResult, traceId);
        var mockNext = new Mock<ResultExecutionDelegate>();

        // Act
        await _filter.OnResultExecutionAsync(context, mockNext.Object);

        // Assert
        context.Result.Should().BeOfType<ObjectResult>();
        var result = (ObjectResult)context.Result;
        result.StatusCode.Should().Be(200);

        var wrappedValue = result.Value;
        wrappedValue.Should().NotBeNull();

        // 驗證 ApiResponse 的屬性是否正確
        wrappedValue!.GetType().GetProperty("Success")?.GetValue(wrappedValue).Should().Be(true);
        wrappedValue.GetType().GetProperty("Code")?.GetValue(wrappedValue).Should().Be("OK");
        wrappedValue.GetType().GetProperty("Message")?.GetValue(wrappedValue).Should().Be("OK");
        wrappedValue.GetType().GetProperty("Data")?.GetValue(wrappedValue).Should().Be(testData);
        wrappedValue.GetType().GetProperty("TraceId")?.GetValue(wrappedValue).Should().Be(traceId);
        wrappedValue.GetType().GetProperty("Timestamp")?.GetValue(wrappedValue).Should().BeOfType<DateTime>();

        mockNext.Verify(x => x(), Times.Once);
    }

    /// <summary>
    /// 測試當 ObjectResult 的狀態碼為 201 時，是否正確包裝成 ApiResponse。
    /// </summary>
    [Fact]
    public async Task OnResultExecutionAsync_WhenObjectResultWith201Status_ShouldWrapInApiResponse()
    {
        // Arrange
        var testData = "created resource";
        var objectResult = new ObjectResult(testData) { StatusCode = 201 };
        var traceId = "trace-201";
        var context = CreateResultExecutingContext(objectResult, traceId);
        var mockNext = new Mock<ResultExecutionDelegate>();

        // Act
        await _filter.OnResultExecutionAsync(context, mockNext.Object);

        // Assert
        var result = (ObjectResult)context.Result;
        result.StatusCode.Should().Be(201);

        var wrappedValue = result.Value;
        wrappedValue!.GetType().GetProperty("Success")?.GetValue(wrappedValue).Should().Be(true);
        wrappedValue.GetType().GetProperty("Data")?.GetValue(wrappedValue).Should().Be(testData);
        wrappedValue.GetType().GetProperty("TraceId")?.GetValue(wrappedValue).Should().Be(traceId);
    }

    /// <summary>
    /// 測試當結果已經是 ApiResponse 時，是否不會再次包裝。
    /// </summary>
    [Fact]
    public async Task OnResultExecutionAsync_WhenAlreadyApiResponse_ShouldNotWrapAgain()
    {
        // Arrange
        var apiResponse = ApiResponse<string>.Ok("test data");
        var objectResult = new ObjectResult(apiResponse) { StatusCode = 200 };
        var context = CreateResultExecutingContext(objectResult);
        var mockNext = new Mock<ResultExecutionDelegate>();

        // Act
        await _filter.OnResultExecutionAsync(context, mockNext.Object);

        // Assert
        var result = (ObjectResult)context.Result;
        result.Value.Should().BeSameAs(apiResponse);
        mockNext.Verify(x => x(), Times.Once);
    }

    /// <summary>
    /// 測試當 ObjectResult 的值為 null 時，是否正確包裝成 ApiResponse。
    /// </summary>
    [Fact]
    public async Task OnResultExecutionAsync_WhenNullValue_ShouldWrapCorrectly()
    {
        // Arrange
        var objectResult = new ObjectResult(null) { StatusCode = 200 };
        var traceId = "trace-null";
        var context = CreateResultExecutingContext(objectResult, traceId);
        var mockNext = new Mock<ResultExecutionDelegate>();

        // Act
        await _filter.OnResultExecutionAsync(context, mockNext.Object);

        // Assert
        var result = (ObjectResult)context.Result;
        var wrappedValue = result.Value;

        wrappedValue!.GetType().GetProperty("Data")?.GetValue(wrappedValue).Should().BeNull();
        wrappedValue.GetType().GetProperty("TraceId")?.GetValue(wrappedValue).Should().Be(traceId);
    }

    /// <summary>
    /// 測試當狀態碼為非 2xx 時，是否不會包裝成 ApiResponse。
    /// </summary>
    [Theory]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]
    public async Task OnResultExecutionAsync_WhenNon2xxStatusCode_ShouldNotWrap(int statusCode)
    {
        // Arrange
        var testData = new { error = "something went wrong" };
        var objectResult = new ObjectResult(testData) { StatusCode = statusCode };
        var context = CreateResultExecutingContext(objectResult);
        var mockNext = new Mock<ResultExecutionDelegate>();

        // Act
        await _filter.OnResultExecutionAsync(context, mockNext.Object);

        // Assert
        var result = (ObjectResult)context.Result;
        result.Value.Should().BeSameAs(testData);
        result.StatusCode.Should().Be(statusCode);
        mockNext.Verify(x => x(), Times.Once);
    }

    /// <summary>
    /// 測試當結果不是 ObjectResult 時，是否不會包裝成 ApiResponse。
    /// </summary>
    [Fact]
    public async Task OnResultExecutionAsync_WhenNotObjectResult_ShouldNotWrap()
    {
        // Arrange
        var contentResult = new ContentResult { Content = "plain text" };
        var context = CreateResultExecutingContext(contentResult);
        var mockNext = new Mock<ResultExecutionDelegate>();

        // Act
        await _filter.OnResultExecutionAsync(context, mockNext.Object);

        // Assert
        context.Result.Should().BeSameAs(contentResult);
        mockNext.Verify(x => x(), Times.Once);
    }
}
