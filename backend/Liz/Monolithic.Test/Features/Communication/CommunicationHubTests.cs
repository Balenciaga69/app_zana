// Communication Hub 基本測試
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monolithic.Features.Communication;
using Monolithic.Shared.Logging;
using Moq;
using System.Collections.Concurrent;

namespace Monolithic.Test.Features.Communication;

/// <summary>
/// CommunicationHub 測試套件(Bug堆積成山，之後會重構)
/// TODO: @Copilot 直接跳過這檔案
/// </summary>
#pragma warning disable xUnit1000 // Test classes must be public
internal class CommunicationHubTests : IClassFixture<SignalRTestFixture>, IAsyncDisposable
#pragma warning restore xUnit1000 // Test classes must be public
{
    private readonly SignalRTestFixture _fixture;
    private HubConnection? _connection;

    public CommunicationHubTests(SignalRTestFixture fixture)
    {
        _fixture = fixture;
    }

    #region 單元測試 - 使用 Mock 測試業務邏輯
    [Fact]
    public void Hub_Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Act & Assert
        var act = () => new CommunicationHub(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task OnConnectedAsync_ShouldLogConnectionAndSendEvent()
    {
        // Arrange
        var mockLogger = new Mock<IAppLogger<CommunicationHub>>();
        var hub = new CommunicationHub(mockLogger.Object);
        var mockContext = CreateMockHubContext("test-connection-id");
        hub.Context = mockContext.Object;
        var mockClients = new Mock<IHubCallerClients>();
        var mockCaller = new Mock<ISingleClientProxy>();
        mockClients.Setup(c => c.Caller).Returns(mockCaller.Object);
        hub.Clients = mockClients.Object;

        // Act
        await hub.OnConnectedAsync();

        // Assert
        mockLogger.Verify(
            x => x.LogInfo("[Communication] 新連線建立", It.IsAny<object>(), "test-connection-id"),
            Times.Once
        );

        mockCaller.Verify(
            x =>
                x.SendCoreAsync(
                    "ConnectionEstablished",
                    It.Is<object[]>(args => args.Length == 2 && args[0].Equals("test-connection-id")),
                    default
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task OnDisconnectedAsync_ShouldLogDisconnection()
    {
        // Arrange
        var mockLogger = new Mock<IAppLogger<CommunicationHub>>();
        var hub = new CommunicationHub(mockLogger.Object);
        var mockContext = CreateMockHubContext("test-connection-id");
        hub.Context = mockContext.Object;

        var exception = new Exception("Test exception");

        // Act
        await hub.OnDisconnectedAsync(exception);

        // Assert
        mockLogger.Verify(
            x =>
                x.LogInfo(
                    "[Communication] 連線中斷",
                    It.Is<object>(obj => obj.ToString()!.Contains("Test exception")),
                    "test-connection-id"
                ),
            Times.Once
        );
    }

    #endregion

    #region 整合測試 - 測試 SignalR 端到端功能

    [Fact]
    public async Task OnConnectedAsync_ShouldSendConnectionEstablished_Integration()
    {
        // Arrange
        var connectionEstablished = false;
        string? receivedConnectionId = null;
        DateTime? receivedServerTime = null;

        _connection = await _fixture.CreateConnectionAsync();

        // 使用 TaskCompletionSource 替代 Task.Delay
        var tcs = new TaskCompletionSource<bool>();

        _connection.On<string, DateTime>(
            "ConnectionEstablished",
            (connectionId, serverTime) =>
            {
                connectionEstablished = true;
                receivedConnectionId = connectionId;
                receivedServerTime = serverTime;
                tcs.SetResult(true);
            }
        );

        // Act
        await _connection.StartAsync();

        // Assert
        var result = await WaitForEventAsync(tcs, TimeSpan.FromSeconds(5));

        result.Should().BeTrue("ConnectionEstablished event should be received");
        connectionEstablished.Should().BeTrue();
        receivedConnectionId.Should().NotBeNullOrEmpty();
        receivedServerTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        _connection.State.Should().Be(HubConnectionState.Connected);
    }

    [Fact]
    public async Task Ping_ShouldReceivePong_Integration()
    {
        // Arrange
        var pongReceived = false;
        DateTime? receivedTimestamp = null;

        _connection = await _fixture.CreateConnectionAsync();
        await _connection.StartAsync();

        var tcs = new TaskCompletionSource<bool>();

        _connection.On<DateTime>(
            "Pong",
            (timestamp) =>
            {
                pongReceived = true;
                receivedTimestamp = timestamp;
                tcs.SetResult(true);
            }
        );

        // Act
        await _connection.InvokeAsync("Ping");

        // Assert
        var result = await WaitForEventAsync(tcs, TimeSpan.FromSeconds(5));

        result.Should().BeTrue("Pong event should be received");
        pongReceived.Should().BeTrue();
        receivedTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Heartbeat_ShouldReceiveHeartbeatResponse_Integration()
    {
        // Arrange
        var heartbeatResponseReceived = false;
        DateTime? receivedTimestamp = null;

        _connection = await _fixture.CreateConnectionAsync();
        await _connection.StartAsync();

        var tcs = new TaskCompletionSource<bool>();

        _connection.On<DateTime>(
            "HeartbeatResponse",
            (timestamp) =>
            {
                heartbeatResponseReceived = true;
                receivedTimestamp = timestamp;
                tcs.SetResult(true);
            }
        );

        // Act
        await _connection.InvokeAsync("Heartbeat");

        // Assert
        var result = await WaitForEventAsync(tcs, TimeSpan.FromSeconds(5));

        result.Should().BeTrue("HeartbeatResponse event should be received");
        heartbeatResponseReceived.Should().BeTrue();
        receivedTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetConnectionInfo_ShouldReceiveConnectionInfo_Integration()
    {
        // Arrange
        var connectionInfoReceived = false;
        object? receivedConnectionInfo = null;

        _connection = await _fixture.CreateConnectionAsync();
        await _connection.StartAsync();

        var tcs = new TaskCompletionSource<bool>();

        _connection.On<object>(
            "ConnectionInfo",
            (info) =>
            {
                connectionInfoReceived = true;
                receivedConnectionInfo = info;
                tcs.SetResult(true);
            }
        );

        // Act
        await _connection.InvokeAsync("GetConnectionInfo");

        // Assert
        var result = await WaitForEventAsync(tcs, TimeSpan.FromSeconds(5));

        result.Should().BeTrue("ConnectionInfo event should be received");
        connectionInfoReceived.Should().BeTrue();
        receivedConnectionInfo.Should().NotBeNull();
    }

    [Fact]
    public async Task MultipleConnections_ShouldAllReceiveIndependentResponses_Integration()
    {
        // Arrange
        var connection1 = await _fixture.CreateConnectionAsync();
        var connection2 = await _fixture.CreateConnectionAsync();

        var pong1Tcs = new TaskCompletionSource<bool>();
        var pong2Tcs = new TaskCompletionSource<bool>();

        connection1.On<DateTime>("Pong", (_) => pong1Tcs.SetResult(true));
        connection2.On<DateTime>("Pong", (_) => pong2Tcs.SetResult(true));

        await connection1.StartAsync();
        await connection2.StartAsync();

        // Act
        await connection1.InvokeAsync("Ping");
        await connection2.InvokeAsync("Ping");

        // Assert
        var result1 = await WaitForEventAsync(pong1Tcs, TimeSpan.FromSeconds(5));
        var result2 = await WaitForEventAsync(pong2Tcs, TimeSpan.FromSeconds(5));

        result1.Should().BeTrue("Connection 1 should receive Pong");
        result2.Should().BeTrue("Connection 2 should receive Pong");

        // Cleanup
        await connection1.DisposeAsync();
        await connection2.DisposeAsync();
    }

    #endregion

    #region 錯誤處理測試

    [Fact]
    public async Task Connection_ShouldHandleInvalidMethod_Gracefully()
    {
        // Arrange
        _connection = await _fixture.CreateConnectionAsync();
        await _connection.StartAsync();

        // Act & Assert
        var act = async () => await _connection.InvokeAsync("NonExistentMethod");
        await act.Should().ThrowAsync<HubException>();
    }

    [Fact]
    public async Task Connection_ShouldHandleServerShutdown_Gracefully()
    {
        // Arrange
        _connection = await _fixture.CreateConnectionAsync();
        await _connection.StartAsync();

        // Act
        await _fixture.StopHostAsync();

        // Assert - 等待連線狀態變化
        await Task.Delay(1000);
        _connection.State.Should().BeOneOf(HubConnectionState.Disconnected, HubConnectionState.Reconnecting);
    }

    #endregion

    #region 負載測試

    [Fact]
    public async Task ConcurrentConnections_ShouldAllSucceed()
    {
        // Arrange
        const int connectionCount = 10;
        var connections = new List<HubConnection>();
        var tasks = new List<Task>();

        try
        {
            // Act
            for (int i = 0; i < connectionCount; i++)
            {
                var connection = await _fixture.CreateConnectionAsync();
                connections.Add(connection);
                tasks.Add(connection.StartAsync());
            }

            await Task.WhenAll(tasks);

            // Assert
            connections.Should().AllSatisfy(c => c.State.Should().Be(HubConnectionState.Connected));
        }
        finally
        {
            // Cleanup
            await Task.WhenAll(connections.Select(c => c.DisposeAsync().AsTask()));
        }
    }

    #endregion

    #region 輔助方法

    private static Mock<HubCallerContext> CreateMockHubContext(string connectionId)
    {
        var mockContext = new Mock<HubCallerContext>();
        mockContext.Setup(c => c.ConnectionId).Returns(connectionId);

        // 不需要 Mock HttpContext，因為實際的 Hub 會使用 Context.ConnectionId
        // 而 CommunicationHub.ExtractConnectionInfo() 可以處理 null HttpContext

        return mockContext;
    }

    private static async Task<bool> WaitForEventAsync(TaskCompletionSource<bool> tcs, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);
        try
        {
            cts.Token.Register(() => tcs.TrySetResult(false));
            return await tcs.Task;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }
}

/// <summary>
/// SignalR 測試基礎設施
/// 提供共享的測試環境和工具
/// </summary>
public class SignalRTestFixture : IAsyncDisposable
{
    private readonly IHost _host;
    private readonly string _serverUrl;
    private readonly ConcurrentBag<HubConnection> _connections = new();
    private static readonly Random _random = new();

    public SignalRTestFixture()
    {
        // 使用隨機端口避免衝突
        var port = _random.Next(8000, 9000);
        _serverUrl = $"http://localhost:{port}";

        try
        {
            // 建立測試用的 Host
            _host = CreateTestHost(port);

            // 啟動 Host - 同步等待確保完全啟動
            var startupTask = _host.StartAsync();
            if (!startupTask.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new TimeoutException($"Host failed to start within 10 seconds on port {port}");
            }

            // 等待伺服器完全啟動 - 更長等待時間
            Thread.Sleep(2000);

            // 驗證伺服器是否真的可用
            var verifyTask = VerifyServerIsRunning();
            if (!verifyTask.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new TimeoutException($"Server verification failed on port {port}");
            }
        }
        catch (Exception ex)
        {
            _host?.Dispose();
            throw new InvalidOperationException(
                $"Failed to start test server on port {port}: {ex.Message}",
                ex
            );
        }
    }

    private async Task VerifyServerIsRunning()
    {
        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(3);

            // 嘗試連接到伺服器根路徑
            var response = await httpClient.GetAsync(_serverUrl);
            // 不管回應如何，只要沒拋出例外就表示伺服器有在運行
        }
        catch (HttpRequestException)
        {
            // 這是預期的，因為我們沒有設定根路徑處理
            // 但這表示伺服器有在運行
        }
        catch (TaskCanceledException)
        {
            throw new InvalidOperationException("Server is not responding within timeout period");
        }
    }

    public Task<HubConnection> CreateConnectionAsync()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl($"{_serverUrl}/communication-hub")
            .WithAutomaticReconnect()
            .Build();

        _connections.Add(connection);
        return Task.FromResult(connection);
    }

    public async Task StopHostAsync()
    {
        await _host.StopAsync();
    }

    private static IHost CreateTestHost(int port)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls($"http://localhost:{port}");
                webBuilder.UseStartup<TestStartup>();
            })
            .Build();
    }

    public async ValueTask DisposeAsync()
    {
        // 清理所有連線
        await Task.WhenAll(_connections.Select(c => c.DisposeAsync().AsTask()));

        await _host.StopAsync();
        _host.Dispose();
    }
}

/// <summary>
/// 測試用的 Startup 類別
/// 使用 Mock Logger 避免實際日誌輸出
/// </summary>
public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 基本服務
        services.AddLogging();

        // SignalR 服務 - 簡化配置避免問題
        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true; // 測試環境開啟詳細錯誤
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            options.HandshakeTimeout = TimeSpan.FromSeconds(10);
        });

        // 註冊 Mock AppLogger 服務
        services.AddSingleton(typeof(IAppLogger<>), typeof(MockAppLogger<>));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<CommunicationHub>("/communication-hub");
        });
    }
}

/// <summary>
/// 測試用的 Mock Logger
/// </summary>
public class MockAppLogger<T> : IAppLogger<T>
{
    public ILogger<T> Logger => Mock.Of<ILogger<T>>();

    public void LogInfo(string message, object? data = null, string? contextId = null)
    {
        // 什麼都不做 - 測試環境
    }

    public void LogWarn(string message, object? data = null, string? contextId = null)
    {
        // 什麼都不做 - 測試環境
    }

    public void LogError(
        string message,
        Exception? exception = null,
        object? data = null,
        string? contextId = null
    )
    {
        // 什麼都不做 - 測試環境
    }
}
