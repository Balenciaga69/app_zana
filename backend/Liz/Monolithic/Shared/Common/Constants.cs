namespace Monolithic.Shared.Common;

/// <summary>
/// 用戶相關常數
/// </summary>
public static class UserConstants
{
    /// <summary>
    /// 用戶暱稱最大長度
    /// </summary>
    public const int MaxNicknameLength = 32;

    /// <summary>
    /// 設備指紋最大長度
    /// </summary>
    public const int MaxDeviceFingerprintLength = 128;

    /// <summary>
    /// 預設暱稱前綴
    /// </summary>
    public const string DefaultNicknamePrefix = "匿名用戶";

    /// <summary>
    /// 用戶連線 ID 最大長度
    /// </summary>
    public const int MaxConnectionIdLength = 128;

    /// <summary>
    /// IP 地址最大長度
    /// </summary>
    public const int MaxIpAddressLength = 45;

    /// <summary>
    /// User Agent 最大長度
    /// </summary>
    public const int MaxUserAgentLength = 500;
}

/// <summary>
/// 房間相關常數
/// </summary>
public static class RoomConstants
{
    /// <summary>
    /// 房間名稱最大長度
    /// </summary>
    public const int MaxNameLength = 100;

    /// <summary>
    /// 邀請碼最大長度
    /// </summary>
    public const int MaxInviteCodeLength = 32;

    /// <summary>
    /// 密碼雜湊最大長度
    /// </summary>
    public const int MaxPasswordHashLength = 256;

    /// <summary>
    /// 預設最大參與人數
    /// </summary>
    public const int DefaultMaxParticipants = 10;
}

/// <summary>
/// 訊息相關常數
/// </summary>
public static class MessageConstants
{
    /// <summary>
    /// 訊息內容最大長度
    /// </summary>
    public const int MaxContentLength = 2000;
}

/// <summary>
/// 應用程式常數
/// </summary>
public static class AppConstants
{
    /// <summary>
    /// 用戶註冊超時時間（分鐘）
    /// </summary>
    public const int UserRegistrationTimeoutMinutes = 5;

    /// <summary>
    /// 房間非活躍清理閾值（分鐘）
    /// </summary>
    public const int RoomInactiveThresholdMinutes = 30;

    /// <summary>
    /// 連線清理保留時間（小時）
    /// </summary>
    public const int ConnectionRetentionHours = 24;

    /// <summary>
    /// 用戶非活躍清理閾值（天）
    /// </summary>
    public const int UserInactiveThresholdDays = 7;
}
