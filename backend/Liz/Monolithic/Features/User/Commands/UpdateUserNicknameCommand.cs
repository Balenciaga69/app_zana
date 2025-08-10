using MediatR;
using Monolithic.Shared.Common;
using Monolithic.Shared.Extensions;

namespace Monolithic.Features.User.Commands;

/// <summary>
/// 更新用戶暱稱命令
/// </summary>
public class UpdateUserNicknameCommand : IRequest<OperationResult<UpdateUserNicknameResult>>
{
    public Guid UserId { get; set; }
    public string Nickname { get; set; } = default!;

    public UpdateUserNicknameCommand(Guid userId, string nickname)
    {
        UserId = userId;
        Nickname = nickname;
    }

    /// <summary>
    /// 驗證命令有效性
    /// </summary>
    public bool IsValid => Nickname.IsValidNickname();

    /// <summary>
    /// 取得驗證錯誤碼
    /// </summary>
    public ErrorCode? GetValidationError()
    {
        if (!Nickname.IsValidNickname())
            return ErrorCode.InvalidNickname;

        return null;
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

    public UpdateUserNicknameResult(Guid userId, string nickname, DateTime updatedAt)
    {
        UserId = userId;
        Nickname = nickname;
        UpdatedAt = updatedAt;
    }
}
