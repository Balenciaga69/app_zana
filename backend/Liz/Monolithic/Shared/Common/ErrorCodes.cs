namespace Monolithic.Shared.Common;

/// <summary>
/// 應用程式錯誤碼枚舉
/// </summary>
public enum ErrorCode
{
    #region 用戶相關錯誤碼

    /// <summary>
    /// 用戶不存在
    /// </summary>
    UserNotFound,

    /// <summary>
    /// 暱稱格式無效
    /// </summary>
    InvalidNickname,

    /// <summary>
    /// 設備指紋無效
    /// </summary>
    InvalidDeviceFingerprint,

    /// <summary>
    /// 用戶未註冊
    /// </summary>
    UserNotRegistered,

    /// <summary>
    /// 用戶已存在
    /// </summary>
    UserAlreadyExists,

    #endregion

    #region 房間相關錯誤碼

    /// <summary>
    /// 房間不存在
    /// </summary>
    RoomNotFound,

    /// <summary>
    /// 房間已滿
    /// </summary>
    RoomFull,

    /// <summary>
    /// 房間密碼錯誤
    /// </summary>
    WrongRoomPassword,

    /// <summary>
    /// 非房間成員
    /// </summary>
    NotRoomMember,

    /// <summary>
    /// 房間名稱無效
    /// </summary>
    InvalidRoomName,

    /// <summary>
    /// 邀請碼無效
    /// </summary>
    InvalidInviteCode,

    #endregion

    #region 連線相關錯誤碼

    /// <summary>
    /// 需要先註冊用戶
    /// </summary>
    AuthRequired,

    /// <summary>
    /// 連線不存在
    /// </summary>
    ConnectionNotFound,

    /// <summary>
    /// 操作頻率過高
    /// </summary>
    RateLimited,

    /// <summary>
    /// 連線已斷開
    /// </summary>
    ConnectionClosed,

    #endregion

    #region 訊息相關錯誤碼

    /// <summary>
    /// 訊息內容無效
    /// </summary>
    InvalidMessageContent,

    /// <summary>
    /// 訊息不存在
    /// </summary>
    MessageNotFound,

    #endregion

    #region 系統錯誤碼

    /// <summary>
    /// 輸入參數無效
    /// </summary>
    InvalidInput,

    /// <summary>
    /// 內部伺服器錯誤
    /// </summary>
    InternalServerError,

    /// <summary>
    /// 資料庫操作失敗
    /// </summary>
    DatabaseError,

    /// <summary>
    /// 服務不可用
    /// </summary>
    ServiceUnavailable

    #endregion
}

/// <summary>
/// 錯誤碼對應的錯誤訊息字典
/// </summary>
public static class ErrorMessages
{
    /// <summary>
    /// 錯誤碼對應訊息的字典
    /// </summary>
    public static readonly Dictionary<ErrorCode, string> Messages = new()
    {
        // 用戶相關錯誤訊息
        [ErrorCode.UserNotFound] = "找不到指定的用戶",
        [ErrorCode.InvalidNickname] = "暱稱格式無效，長度必須在1-32個字元之間",
        [ErrorCode.InvalidDeviceFingerprint] = "設備指紋格式無效",
        [ErrorCode.UserNotRegistered] = "用戶尚未註冊，請先完成註冊",
        [ErrorCode.UserAlreadyExists] = "該設備指紋已註冊用戶",

        // 房間相關錯誤訊息
        [ErrorCode.RoomNotFound] = "找不到指定的房間",
        [ErrorCode.RoomFull] = "房間人數已滿，無法加入",
        [ErrorCode.WrongRoomPassword] = "房間密碼錯誤",
        [ErrorCode.NotRoomMember] = "您不是該房間的成員",
        [ErrorCode.InvalidRoomName] = "房間名稱格式無效",
        [ErrorCode.InvalidInviteCode] = "邀請碼格式無效",

        // 連線相關錯誤訊息
        [ErrorCode.AuthRequired] = "請先註冊用戶身份",
        [ErrorCode.ConnectionNotFound] = "找不到指定的連線",
        [ErrorCode.RateLimited] = "操作頻率過高，請稍後再試",
        [ErrorCode.ConnectionClosed] = "連線已斷開",

        // 訊息相關錯誤訊息
        [ErrorCode.InvalidMessageContent] = "訊息內容格式無效",
        [ErrorCode.MessageNotFound] = "找不到指定的訊息",

        // 系統錯誤訊息
        [ErrorCode.InvalidInput] = "輸入參數格式無效",
        [ErrorCode.InternalServerError] = "伺服器發生未預期錯誤，請稍後再試",
        [ErrorCode.DatabaseError] = "資料庫操作失敗",
        [ErrorCode.ServiceUnavailable] = "服務暫時不可用",
    };

    /// <summary>
    /// 取得錯誤碼對應的錯誤訊息
    /// </summary>
    /// <param name="errorCode">錯誤碼</param>
    /// <returns>錯誤訊息</returns>
    public static string GetMessage(ErrorCode errorCode)
    {
        return Messages.TryGetValue(errorCode, out var message) ? message : "未知錯誤";
    }
}
