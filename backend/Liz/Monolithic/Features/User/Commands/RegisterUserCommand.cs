using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.Communication;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Commands;

public class RegisterUserCommand : IRequest
{
    public string? ExistingUserId { get; set; }
    public string DeviceFingerprint { get; set; }
    public string ConnectionId { get; set; }

    public RegisterUserCommand(string? existingUserId, string deviceFingerprint, string connectionId)
    {
        ExistingUserId = existingUserId;
        DeviceFingerprint = deviceFingerprint;
        ConnectionId = connectionId;
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IHubContext<CommunicationHub> _hubContext;
    private readonly IAppLogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IHubContext<CommunicationHub> hubContext,
        IAppLogger<RegisterUserCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        // TODO: 實作用戶註冊/重連邏輯
        // 1. 檢查是否為重連（existingUserId + deviceFingerprint 驗證）
        // 2. 創建新用戶或恢復現有用戶
        // 3. 記錄 UserConnection
        // 4. 發送 UserRegistered 事件
        await _hubContext
            .Clients.Client(command.ConnectionId)
            .SendAsync("UserRegistered", "user123", "匿名用戶", true);
    }
}
