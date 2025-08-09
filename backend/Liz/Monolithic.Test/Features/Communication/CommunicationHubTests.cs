// Communication Hub 基本測試
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Monolithic.Test.Features.Communication;

/// <summary>
/// CommunicationHub 基本功能測試
/// 遵循 TDD 原則，確保 SignalR 連線和基本功能正常
/// </summary>
public class CommunicationHubTests : IAsyncDisposable
{
    private readonly IHost _host;
    private readonly string _serverUrl;
    private HubConnection? _connection;

    public CommunicationHubTests()
    {
        // 建立測試用的 Host
        _host = CreateTestHost();
        _serverUrl = "http://localhost:5000";
    }

    [Fact]
    public async Task OnConnectedAsync_ShouldSendConnectionEstablished()
    {
        // Arrange
        var connectionEstablishedReceived = false;
        string? receivedConnectionId = null;
        DateTime? receivedServerTime = null;

        _connection = await CreateConnection();

        _connection.On<string, DateTime>("ConnectionEstablished", (connectionId, serverTime) =>
        {
            connectionEstablishedReceived = true;
            receivedConnectionId = connectionId;
            receivedServerTime = serverTime;
        });

        // Act
        await _connection.StartAsync();

        // Assert
        await Task.Delay(1000); // 等待事件處理

        connectionEstablishedReceived.Should().BeTrue();
        receivedConnectionId.Should().NotBeNullOrEmpty();
        receivedServerTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        _connection.State.Should().Be(HubConnectionState.Connected);
    }

    [Fact]
    public async Task Ping_ShouldReceivePong()
    {
        // Arrange
        var pongReceived = false;
        DateTime? receivedTimestamp = null;

        _connection = await CreateConnection();
        await _connection.StartAsync();

        _connection.On<DateTime>("Pong", (timestamp) =>
        {
            pongReceived = true;
            receivedTimestamp = timestamp;
        });

        // Act
        await _connection.InvokeAsync("Ping");

        // Assert
        await Task.Delay(1000); // 等待回應

        pongReceived.Should().BeTrue();
        receivedTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Heartbeat_ShouldReceiveHeartbeatResponse()
    {
        // Arrange
        var heartbeatResponseReceived = false;
        DateTime? receivedTimestamp = null;

        _connection = await CreateConnection();
        await _connection.StartAsync();

        _connection.On<DateTime>("HeartbeatResponse", (timestamp) =>
        {
            heartbeatResponseReceived = true;
            receivedTimestamp = timestamp;
        });

        // Act
        await _connection.InvokeAsync("Heartbeat");

        // Assert
        await Task.Delay(1000); // 等待回應

        heartbeatResponseReceived.Should().BeTrue();
        receivedTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetConnectionInfo_ShouldReceiveConnectionInfo()
    {
        // Arrange
        var connectionInfoReceived = false;
        object? receivedConnectionInfo = null;

        _connection = await CreateConnection();
        await _connection.StartAsync();

        _connection.On<object>("ConnectionInfo", (info) =>
        {
            connectionInfoReceived = true;
            receivedConnectionInfo = info;
        });

        // Act
        await _connection.InvokeAsync("GetConnectionInfo");

        // Assert
        await Task.Delay(1000); // 等待回應

        connectionInfoReceived.Should().BeTrue();
        receivedConnectionInfo.Should().NotBeNull();
    }

    [Fact]
    public async Task MultipleConnections_ShouldAllReceiveIndependentResponses()
    {
        // Arrange
        var connection1 = await CreateConnection();
        var connection2 = await CreateConnection();

        var pong1Received = false;
        var pong2Received = false;

        connection1.On<DateTime>("Pong", (_) => pong1Received = true);
        connection2.On<DateTime>("Pong", (_) => pong2Received = true);

        await connection1.StartAsync();
        await connection2.StartAsync();

        // Act
        await connection1.InvokeAsync("Ping");
        await connection2.InvokeAsync("Ping");

        // Assert
        await Task.Delay(1000);

        pong1Received.Should().BeTrue();
        pong2Received.Should().BeTrue();

        // Cleanup
        await connection1.DisposeAsync();
        await connection2.DisposeAsync();
    }

    /// <summary>
    /// 建立測試用的 SignalR 連線
    /// </summary>
    private async Task<HubConnection> CreateConnection()
    {
        return await Task.FromResult(new HubConnectionBuilder()
            .WithUrl("http://localhost/communication-hub")
            .Build());
    }

    /// <summary>
    /// 建立測試用的 Host
    /// </summary>
    private static IHost CreateTestHost()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://localhost:5000");
                webBuilder.UseStartup<TestStartup>();
            })
            .Build();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }

        await _host.StopAsync();
        _host.Dispose();
    }
}

/// <summary>
/// 測試用的 Startup 類別
/// 簡化版的服務註冊，僅用於測試
/// </summary>
public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 基本服務
        services.AddLogging();
        services.AddSignalR();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            // endpoints.MapHub<CommunicationHub>("/communication-hub");
        });
    }
}
