using MediatR;

namespace Monolithic.Features.Identity.Requests;

/// <summary>
/// 設定用戶上線狀態的 Command
/// </summary>
public class SetUserOnlineStatusCommand : IRequest<Unit>
{
    public Guid UserId { get; }
    public bool IsOnline { get; }

    public SetUserOnlineStatusCommand(Guid userId, bool isOnline)
    {
        UserId = userId;
        IsOnline = isOnline;
    }
}
