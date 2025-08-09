namespace Monolithic.Shared.Common;

/// <summary>
/// 操作結果基類
/// </summary>
/// <typeparam name="T">資料類型</typeparam>
public class OperationResult<T>
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorCode { get; private set; }
    public string? ErrorMessage { get; private set; }
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

    protected OperationResult(bool success, T? data, string? errorCode, string? errorMessage)
    {
        Success = success;
        Data = data;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// 建立成功結果
    /// </summary>
    public static OperationResult<T> Ok(T data) => new(true, data, null, null);

    /// <summary>
    /// 建立失敗結果
    /// </summary>
    public static OperationResult<T> Fail(string errorCode, string errorMessage) => new(false, default, errorCode, errorMessage);

    /// <summary>
    /// 建立失敗結果（僅錯誤碼）
    /// </summary>
    public static OperationResult<T> Fail(string errorCode) => new(false, default, errorCode, GetErrorMessage(errorCode));

    /// <summary>
    /// 根據錯誤碼取得錯誤訊息
    /// </summary>
    private static string GetErrorMessage(string errorCode) =>
        errorCode switch
        {
            ErrorCodes.UserNotFound => ErrorMessages.UserNotFound,
            ErrorCodes.InvalidNickname => ErrorMessages.InvalidNickname,
            ErrorCodes.InvalidDeviceFingerprint => ErrorMessages.InvalidDeviceFingerprint,
            ErrorCodes.UserNotRegistered => ErrorMessages.UserNotRegistered,
            ErrorCodes.UserAlreadyExists => ErrorMessages.UserAlreadyExists,
            ErrorCodes.RoomNotFound => ErrorMessages.RoomNotFound,
            ErrorCodes.RoomFull => ErrorMessages.RoomFull,
            ErrorCodes.WrongRoomPassword => ErrorMessages.WrongRoomPassword,
            ErrorCodes.NotRoomMember => ErrorMessages.NotRoomMember,
            ErrorCodes.InvalidRoomName => ErrorMessages.InvalidRoomName,
            ErrorCodes.InvalidInviteCode => ErrorMessages.InvalidInviteCode,
            ErrorCodes.AuthRequired => ErrorMessages.AuthRequired,
            ErrorCodes.ConnectionNotFound => ErrorMessages.ConnectionNotFound,
            ErrorCodes.RateLimited => ErrorMessages.RateLimited,
            ErrorCodes.ConnectionClosed => ErrorMessages.ConnectionClosed,
            ErrorCodes.InvalidMessageContent => ErrorMessages.InvalidMessageContent,
            ErrorCodes.MessageNotFound => ErrorMessages.MessageNotFound,
            ErrorCodes.InvalidInput => ErrorMessages.InvalidInput,
            ErrorCodes.InternalServerError => ErrorMessages.InternalServerError,
            ErrorCodes.DatabaseError => ErrorMessages.DatabaseError,
            ErrorCodes.ServiceUnavailable => ErrorMessages.ServiceUnavailable,
            _ => "未知錯誤",
        };

    /// <summary>
    /// 轉換為 ApiResponse 格式
    /// </summary>
    public ApiResponse<T> ToApiResponse()
    {
        if (Success)
        {
            return ApiResponse<T>.Ok(Data!, "操作成功");
        }
        else
        {
            return ApiResponse<T>.Fail(ErrorCode!, ErrorMessage!);
        }
    }
}

/// <summary>
/// 不含資料的操作結果
/// </summary>
public class OperationResult : OperationResult<object>
{
    private OperationResult(bool success, string? errorCode, string? errorMessage)
        : base(success, null, errorCode, errorMessage) { }

    /// <summary>
    /// 建立成功結果
    /// </summary>
    public static OperationResult Ok() => new(true, null, null);

    /// <summary>
    /// 建立失敗結果
    /// </summary>
    public static new OperationResult Fail(string errorCode, string errorMessage) => new(false, errorCode, errorMessage);

    /// <summary>
    /// 建立失敗結果（僅錯誤碼）
    /// </summary>
    public static new OperationResult Fail(string errorCode) => new(false, errorCode, GetErrorMessage(errorCode));

    private static string GetErrorMessage(string errorCode) =>
        errorCode switch
        {
            ErrorCodes.UserNotFound => ErrorMessages.UserNotFound,
            ErrorCodes.InvalidNickname => ErrorMessages.InvalidNickname,
            ErrorCodes.InvalidDeviceFingerprint => ErrorMessages.InvalidDeviceFingerprint,
            ErrorCodes.UserNotRegistered => ErrorMessages.UserNotRegistered,
            ErrorCodes.UserAlreadyExists => ErrorMessages.UserAlreadyExists,
            ErrorCodes.RoomNotFound => ErrorMessages.RoomNotFound,
            ErrorCodes.RoomFull => ErrorMessages.RoomFull,
            ErrorCodes.WrongRoomPassword => ErrorMessages.WrongRoomPassword,
            ErrorCodes.NotRoomMember => ErrorMessages.NotRoomMember,
            ErrorCodes.InvalidRoomName => ErrorMessages.InvalidRoomName,
            ErrorCodes.InvalidInviteCode => ErrorMessages.InvalidInviteCode,
            ErrorCodes.AuthRequired => ErrorMessages.AuthRequired,
            ErrorCodes.ConnectionNotFound => ErrorMessages.ConnectionNotFound,
            ErrorCodes.RateLimited => ErrorMessages.RateLimited,
            ErrorCodes.ConnectionClosed => ErrorMessages.ConnectionClosed,
            ErrorCodes.InvalidMessageContent => ErrorMessages.InvalidMessageContent,
            ErrorCodes.MessageNotFound => ErrorMessages.MessageNotFound,
            ErrorCodes.InvalidInput => ErrorMessages.InvalidInput,
            ErrorCodes.InternalServerError => ErrorMessages.InternalServerError,
            ErrorCodes.DatabaseError => ErrorMessages.DatabaseError,
            ErrorCodes.ServiceUnavailable => ErrorMessages.ServiceUnavailable,
            _ => "未知錯誤",
        };
}
