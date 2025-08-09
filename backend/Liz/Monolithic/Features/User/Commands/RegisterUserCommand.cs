using MediatR;

namespace Monolithic.Features.User.Commands;

/// <summary>
/// 用戶註冊命令 - 包含命令和結果定義
/// </summary>
public class RegisterUserCommand : IRequest<RegisterUserResult>
{
    /// <summary>
    /// 現有用戶 ID（重新連線時使用）
    /// </summary>
    public Guid? ExistingUserId { get; set; }

    /// <summary>
    /// 設備指紋（必填）
    /// </summary>
    public string DeviceFingerprint { get; set; } = default!;

    /// <summary>
    /// 用戶代理字串（可選）
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// IP 地址（可選）
    /// </summary>
    public string? IpAddress { get; set; }
}

/// <summary>
/// 用戶註冊命令結果
/// </summary>
public class RegisterUserResult
{
    /// <summary>
    /// 用戶 ID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 用戶暱稱
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 是否為新用戶
    /// </summary>
    public bool IsNewUser { get; set; }

    /// <summary>
    /// 用戶是否啟用
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 最後活動時間
    /// </summary>
    public DateTime LastActiveAt { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
