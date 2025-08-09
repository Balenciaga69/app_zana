using MediatR;

namespace Monolithic.Features.User.Commands;

/// <summary>
/// 更新用戶暱稱命令
/// </summary>
public class UpdateUserNicknameCommand : IRequest<UpdateUserNicknameResult>
{
    public Guid UserId { get; set; }
    public string Nickname { get; set; } = default!;

    public UpdateUserNicknameCommand(Guid userId, string nickname)
    {
        UserId = userId;
        Nickname = nickname;
    }
}

/// <summary>
/// 更新用戶暱稱命令結果
/// </summary>
public class UpdateUserNicknameResult
{
    public Guid UserId { get; set; }
    public string Nickname { get; set; } = default!;
    public DateTime UpdatedAt { get; set; }
    public bool Success { get; set; }
}
