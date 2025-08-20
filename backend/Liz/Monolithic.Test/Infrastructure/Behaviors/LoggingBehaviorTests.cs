using FluentAssertions;
using Infrastructure.Behaviors;
using MediatR;
using Monolithic.Shared.Logging;
using Moq;

namespace Monolithic.Test.Infrastructure.Behaviors;

public class LoggingBehaviorTests
{
    public class DummyRequest { }

    public class DummyResponse { }

    [Fact]
    /*
     驗證 LoggingBehavior
     在處理過程中成功時
     應該記錄開始和結束的日誌，並返回響應
     */
    public async Task Handle_Should_Log_Begin_And_End_With_Same_TraceId_And_Return_Response_When_Success()
    {
        // Arrange
        var loggerMock = new Mock<IAppLogger<LoggingBehavior<DummyRequest, DummyResponse>>>();
        var infoCalls = new List<(string Message, object? Data, string? Trace)>();
        loggerMock
            .Setup(l => l.LogInfo(It.IsAny<string>(), It.IsAny<object?>(), It.IsAny<string?>()))
            .Callback((string m, object? d, string? t) => infoCalls.Add((m, d, t)));

        var sut = new LoggingBehavior<DummyRequest, DummyResponse>(loggerMock.Object);
        var request = new DummyRequest();
        var response = new DummyResponse();
        RequestHandlerDelegate<DummyResponse> next = (ct) => Task.FromResult(response);

        // Act
        var result = await sut.Handle(request, next, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(response);
        infoCalls.Count.Should().Be(2, "should log begin and end");

        var begin = infoCalls[0];
        var end = infoCalls[1];

        begin.Message.Should().Be("Handling DummyRequest");
        begin.Data.Should().BeSameAs(request);
        begin.Trace.Should().NotBeNullOrWhiteSpace();

        end.Message.Should().StartWith("Handled DummyRequest in ");
        end.Message.Should().Contain("ms");
        end.Data.Should().BeSameAs(response);
        end.Trace.Should().Be(begin.Trace, "trace id must remain the same");

        loggerMock.Verify(
            l => l.LogInfo(It.IsAny<string>(), It.IsAny<object?>(), It.IsAny<string?>()),
            Times.Exactly(2)
        );
        loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    /*
     驗證 LoggingBehavior
     在處理過程中發生異常時
     應該記錄開始和錯誤的日誌，並重新拋出異常
     */
    public async Task Handle_Should_Log_Begin_And_Error_With_Same_TraceId_And_Rethrow_When_Exception()
    {
        // Arrange
        var loggerMock = new Mock<IAppLogger<LoggingBehavior<DummyRequest, DummyResponse>>>();
        var infoCalls = new List<(string Message, object? Data, string? Trace)>();
        (string Message, Exception? Exception, object? Data, string? Trace) errorCall = default;
        var errorCalled = false;

        loggerMock
            .Setup(l => l.LogInfo(It.IsAny<string>(), It.IsAny<object?>(), It.IsAny<string?>()))
            .Callback((string m, object? d, string? t) => infoCalls.Add((m, d, t)));

        loggerMock
            .Setup(l =>
                l.LogError(
                    It.IsAny<string>(),
                    It.IsAny<Exception?>(),
                    It.IsAny<object?>(),
                    It.IsAny<string?>()
                )
            )
            .Callback(
                (string m, Exception? e, object? d, string? t) =>
                {
                    errorCall = (m, e!, d, t);
                    errorCalled = true;
                }
            );

        var sut = new LoggingBehavior<DummyRequest, DummyResponse>(loggerMock.Object);
        var request = new DummyRequest();
        var ex = new InvalidOperationException("boom");
        RequestHandlerDelegate<DummyResponse> next = (ct) => Task.FromException<DummyResponse>(ex);

        // Act
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.Handle(request, next, CancellationToken.None)
        );

        // Assert
        thrown.Should().BeSameAs(ex);
        infoCalls.Count.Should().Be(1, "should only log begin on failure");
        errorCalled.Should().BeTrue();

        var begin = infoCalls[0];
        begin.Message.Should().Be("Handling DummyRequest");
        begin.Data.Should().BeSameAs(request);
        begin.Trace.Should().NotBeNullOrWhiteSpace();

        errorCall.Message.Should().StartWith("Error handling DummyRequest after ");
        errorCall.Message.Should().Contain("ms");
        errorCall.Exception.Should().BeSameAs(ex);
        errorCall.Data.Should().BeSameAs(request);
        errorCall.Trace.Should().Be(begin.Trace);

        loggerMock.Verify(
            l => l.LogInfo(It.IsAny<string>(), It.IsAny<object?>(), It.IsAny<string?>()),
            Times.Once
        );
        loggerMock.Verify(
            l =>
                l.LogError(
                    It.IsAny<string>(),
                    It.IsAny<Exception?>(),
                    It.IsAny<object?>(),
                    It.IsAny<string?>()
                ),
            Times.Once
        );
        loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    /*
    驗證 LoggingBehavior
    在開始時
    將 request 作為 Data 傳給 logger
    結束時
    將 response 作為 Data 傳給 logger
    */
    public async Task Handle_Should_Pass_Request_As_Data_On_Begin_And_Response_On_End()
    {
        // Arrange
        var loggerMock = new Mock<IAppLogger<LoggingBehavior<DummyRequest, DummyResponse>>>();
        var infoCalls = new List<(string Message, object? Data, string? Trace)>();
        loggerMock
            .Setup(l => l.LogInfo(It.IsAny<string>(), It.IsAny<object?>(), It.IsAny<string?>()))
            .Callback((string m, object? d, string? t) => infoCalls.Add((m, d, t)));

        var sut = new LoggingBehavior<DummyRequest, DummyResponse>(loggerMock.Object);
        var request = new DummyRequest();
        var response = new DummyResponse();
        RequestHandlerDelegate<DummyResponse> next = (ct) => Task.FromResult(response);

        // Act
        await sut.Handle(request, next, CancellationToken.None);

        // Assert
        infoCalls.Should().HaveCount(2);
        infoCalls[0].Data.Should().BeSameAs(request);
        infoCalls[1].Data.Should().BeSameAs(response);
    }

    [Fact]
    /*
     驗證 LoggingBehavior
     在結束時的 log message
     會包含執行所花費的毫秒數(ms)
     */
    public async Task Handle_Should_Include_ElapsedMilliseconds_In_End_Log_Message()
    {
        // Arrange
        var loggerMock = new Mock<IAppLogger<LoggingBehavior<DummyRequest, DummyResponse>>>();
        var infoCalls = new List<(string Message, object? Data, string? Trace)>();
        loggerMock
            .Setup(l => l.LogInfo(It.IsAny<string>(), It.IsAny<object?>(), It.IsAny<string?>()))
            .Callback((string m, object? d, string? t) => infoCalls.Add((m, d, t)));

        var sut = new LoggingBehavior<DummyRequest, DummyResponse>(loggerMock.Object);
        var request = new DummyRequest();
        var response = new DummyResponse();
        RequestHandlerDelegate<DummyResponse> next = (ct) => Task.FromResult(response);

        // Act
        await sut.Handle(request, next, CancellationToken.None);

        // Assert
        infoCalls.Should().HaveCount(2);
        var end = infoCalls[1];
        end.Message.Should().StartWith("Handled DummyRequest in ");
        end.Message.Should().Contain("ms");
    }
}
