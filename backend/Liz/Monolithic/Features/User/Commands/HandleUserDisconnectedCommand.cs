using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.Communication;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Commands;

public class HandleUserDisconnectedCommand : IRequest
{
    public string ConnectionId { get; set; }
    public string? Reason { get; set; }

    public HandleUserDisconnectedCommand(string connectionId, string? reason = null)
    {
        ConnectionId = connectionId;
        Reason = reason;
    }
}

public class HandleUserDisconnectedCommandHandler : IRequestHandler<HandleUserDisconnectedCommand>
{
    private readonly IUserConnectionRepository _userConnectionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHubContext<CommunicationHub> _hubContext;
    private readonly IAppLogger<HandleUserDisconnectedCommandHandler> _logger;

    public HandleUserDisconnectedCommandHandler(
        IUserConnectionRepository userConnectionRepository,
        IUserRepository userRepository,
        IHubContext<CommunicationHub> hubContext,
        IAppLogger<HandleUserDisconnectedCommandHandler> logger
    )
    {
        _userConnectionRepository = userConnectionRepository;
        _userRepository = userRepository;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task Handle(HandleUserDisconnectedCommand request, CancellationToken cancellationToken)
    {
        // 查詢該 ConnectionId 對應的 UserConnection
        var userConnection = await _userConnectionRepository.GetByConnectionIdAsync(request.ConnectionId);
        if (userConnection == null)
        {
            return;
        }

        // 標記 DisconnectedAt
        userConnection.DisconnectedAt = DateTime.UtcNow;
        await _userConnectionRepository.UpdateConnectionAsync(userConnection);

        // 檢查用戶是否還有其他活躍連線
        var userId = userConnection.UserId;
        var allConnections = await _userConnectionRepository.GetConnectionsByUserIdAsync(Guid.Parse(userId));
        var stillActive = allConnections.Any(c =>
            c.DisconnectedAt == null || c.DisconnectedAt > DateTime.UtcNow.AddMinutes(-1)
        );

        // 如果沒有，更新用戶狀態為離線
        if (!stillActive)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
            if (user != null)
            {
                user.IsActive = false;
                user.LastActiveAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
            }
        }

        // 廣播 UserStatusChanged（可依需求擴充房間通知）
        await _hubContext.Clients.All.SendAsync("UserStatusChanged", userId, false);

        // 可擴充：查詢該用戶參與的房間，逐一離開（如有房間管理需求）
        // ...

        return;
    }
}
