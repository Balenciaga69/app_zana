namespace Monolithic.Shared.Common;

/// <summary>
/// 應用程式錯誤碼定義
/// </summary>
public static class ErrorCodes
{
    #region 用戶相關錯誤碼

    /// <summary>
    /// 用戶不存在
    /// </summary>
    public const string UserNotFound = "USER_NOT_FOUND";

    /// <summary>
    /// 暱稱格式無效
    /// </summary>
    public const string InvalidNickname = "INVALID_NICKNAME";

    /// <summary>
    /// 設備指紋無效
    /// </summary>
    public const string InvalidDeviceFingerprint = "INVALID_DEVICE_FINGERPRINT";

    /// <summary>
    /// 用戶未註冊
    /// </summary>
    public const string UserNotRegistered = "USER_NOT_REGISTERED";

    /// <summary>
    /// 用戶已存在
    /// </summary>
    public const string UserAlreadyExists = "USER_ALREADY_EXISTS";

    #endregion

    #region 房間相關錯誤碼

    /// <summary>
    /// 房間不存在
    /// </summary>
    public const string RoomNotFound = "ROOM_NOT_FOUND";

    /// <summary>
    /// 房間已滿
    /// </summary>
    public const string RoomFull = "ROOM_FULL";

    /// <summary>
    /// 房間密碼錯誤
    /// </summary>
    public const string WrongRoomPassword = "WRONG_ROOM_PASSWORD";

    /// <summary>
    /// 非房間成員
    /// </summary>
    public const string NotRoomMember = "NOT_ROOM_MEMBER";

    /// <summary>
    /// 房間名稱無效
    /// </summary>
    public const string InvalidRoomName = "INVALID_ROOM_NAME";

    /// <summary>
    /// 邀請碼無效
    /// </summary>
    public const string InvalidInviteCode = "INVALID_INVITE_CODE";

    #endregion

    #region 連線相關錯誤碼

    /// <summary>
    /// 需要先註冊用戶
    /// </summary>
    public const string AuthRequired = "AUTH_REQUIRED";

    /// <summary>
    /// 連線不存在
    /// </summary>
    public const string ConnectionNotFound = "CONNECTION_NOT_FOUND";

    /// <summary>
    /// 操作頻率過高
    /// </summary>
    public const string RateLimited = "RATE_LIMITED";

    /// <summary>
    /// 連線已斷開
    /// </summary>
    public const string ConnectionClosed = "CONNECTION_CLOSED";

    #endregion

    #region 訊息相關錯誤碼

    /// <summary>
    /// 訊息內容無效
    /// </summary>
    public const string InvalidMessageContent = "INVALID_MESSAGE_CONTENT";

    /// <summary>
    /// 訊息不存在
    /// </summary>
    public const string MessageNotFound = "MESSAGE_NOT_FOUND";

    #endregion

    #region 系統錯誤碼

    /// <summary>
    /// 輸入參數無效
    /// </summary>
    public const string InvalidInput = "INVALID_INPUT";

    /// <summary>
    /// 內部伺服器錯誤
    /// </summary>
    public const string InternalServerError = "INTERNAL_SERVER_ERROR";

    /// <summary>
    /// 資料庫操作失敗
    /// </summary>
    public const string DatabaseError = "DATABASE_ERROR";

    /// <summary>
    /// 服務不可用
    /// </summary>
    public const string ServiceUnavailable = "SERVICE_UNAVAILABLE";

    #endregion
}

/// <summary>
/// 錯誤訊息定義
/// </summary>
public static class ErrorMessages
{
    #region 用戶相關錯誤訊息

    public const string UserNotFound = "找不到指定的用戶";
    public const string InvalidNickname = "暱稱格式無效，長度必須在1-32個字元之間";
    public const string InvalidDeviceFingerprint = "設備指紋格式無效";
    public const string UserNotRegistered = "用戶尚未註冊，請先完成註冊";
    public const string UserAlreadyExists = "該設備指紋已註冊用戶";

    #endregion

    #region 房間相關錯誤訊息

    public const string RoomNotFound = "找不到指定的房間";
    public const string RoomFull = "房間人數已滿，無法加入";
    public const string WrongRoomPassword = "房間密碼錯誤";
    public const string NotRoomMember = "您不是該房間的成員";
    public const string InvalidRoomName = "房間名稱格式無效";
    public const string InvalidInviteCode = "邀請碼格式無效";

    #endregion

    #region 連線相關錯誤訊息

    public const string AuthRequired = "請先註冊用戶身份";
    public const string ConnectionNotFound = "找不到指定的連線";
    public const string RateLimited = "操作頻率過高，請稍後再試";
    public const string ConnectionClosed = "連線已斷開";

    #endregion

    #region 訊息相關錯誤訊息

    public const string InvalidMessageContent = "訊息內容格式無效";
    public const string MessageNotFound = "找不到指定的訊息";

    #endregion

    #region 系統錯誤訊息

    public const string InvalidInput = "輸入參數格式無效";
    public const string InternalServerError = "伺服器發生未預期錯誤，請稍後再試";
    public const string DatabaseError = "資料庫操作失敗";
    public const string ServiceUnavailable = "服務暫時不可用";

    #endregion
}
