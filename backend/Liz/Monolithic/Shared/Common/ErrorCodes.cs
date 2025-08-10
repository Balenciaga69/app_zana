namespace Monolithic.Shared.Common;

/// <summary>
/// 應用程式錯誤碼枚舉
/// </summary>
public enum ErrorCode
{
    AuthRequired,
    NotFound,
    InvalidDeviceFingerprint,
    InvalidInput,
    InternalServerError,
    ResourceNotFound,
}

/// <summary>
/// 錯誤碼對應的錯誤訊息字典
/// </summary>
public static class ErrorMessages
{
    public static readonly Dictionary<ErrorCode, string> Messages = new()
    {
        [ErrorCode.AuthRequired] = "需要認證",
        [ErrorCode.NotFound] = "找不到資源",
        [ErrorCode.InvalidInput] = "輸入參數無效",
        [ErrorCode.InternalServerError] = "內部伺服器錯誤",
        [ErrorCode.ResourceNotFound] = "資源不存在",
    };

    public static string GetMessage(ErrorCode errorCode)
    {
        return Messages.TryGetValue(errorCode, out var message) ? message : "未知錯誤";
    }
}
