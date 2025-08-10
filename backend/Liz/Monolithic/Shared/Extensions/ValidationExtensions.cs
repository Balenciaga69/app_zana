using Monolithic.Shared.Common;

namespace Monolithic.Shared.Extensions;

/// <summary>
/// 驗證擴展方法
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// 驗證用戶暱稱
    /// </summary>
    public static bool IsValidNickname(this string? nickname)
    {
        return !string.IsNullOrWhiteSpace(nickname) && nickname.Length <= UserConstants.MaxNicknameLength;
    }

    /// <summary>
    /// 驗證設備指紋
    /// </summary>
    public static bool IsValidDeviceFingerprint(this string? deviceFingerprint)
    {
        return !string.IsNullOrWhiteSpace(deviceFingerprint)
            && deviceFingerprint.Length <= UserConstants.MaxDeviceFingerprintLength;
    }

    /// <summary>
    /// 驗證房間名稱
    /// </summary>
    public static bool IsValidRoomName(this string? roomName)
    {
        return !string.IsNullOrWhiteSpace(roomName) && roomName.Length <= RoomConstants.MaxNameLength;
    }

    /// <summary>
    /// 驗證訊息內容
    /// </summary>
    public static bool IsValidMessageContent(this string? content)
    {
        return !string.IsNullOrWhiteSpace(content) && content.Length <= MessageConstants.MaxContentLength;
    }

    /// <summary>
    /// 驗證邀請碼
    /// </summary>
    public static bool IsValidInviteCode(this string? inviteCode)
    {
        return !string.IsNullOrWhiteSpace(inviteCode)
            && inviteCode.Length <= RoomConstants.MaxInviteCodeLength;
    }

    /// <summary>
    /// 驗證 GUID
    /// </summary>
    public static bool IsValidGuid(this Guid? guid)
    {
        return guid.HasValue && guid.Value != Guid.Empty;
    }

    /// <summary>
    /// 驗證 GUID
    /// </summary>
    public static bool IsValidGuid(this Guid guid)
    {
        return guid != Guid.Empty;
    }
}
