namespace Monolithic.Shared.Common;

/// <summary>
/// 操作結果基類
/// </summary>
/// <typeparam name="T">資料類型</typeparam>
public class OperationResult<T>
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public ErrorCode? ErrorCode { get; private set; }
    public string? ErrorMessage { get; private set; }
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

    protected OperationResult(bool success, T? data, ErrorCode? errorCode, string? errorMessage)
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
    /// 建立失敗結果（使用 ErrorCode 枚舉）
    /// </summary>
    public static OperationResult<T> Fail(ErrorCode errorCode) => new(false, default, errorCode, ErrorMessages.GetMessage(errorCode));

    /// <summary>
    /// 建立失敗結果（使用 ErrorCode 枚舉和自定義訊息）
    /// </summary>
    public static OperationResult<T> Fail(ErrorCode errorCode, string customMessage) => new(false, default, errorCode, customMessage);

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
            if (ErrorCode.HasValue)
            {
                return ApiResponse<T>.Fail(ErrorCode.Value, ErrorMessage!);
            }
            else
            {
                // 向後相容性支援
                return ApiResponse<T>.Fail("Unknown", ErrorMessage ?? "未知錯誤");
            }
        }
    }
}

/// <summary>
/// 不含資料的操作結果
/// </summary>
public class OperationResult : OperationResult<object>
{
    private OperationResult(bool success, ErrorCode? errorCode, string? errorMessage)
        : base(success, null, errorCode, errorMessage) { }

    /// <summary>
    /// 建立成功結果
    /// </summary>
    public static OperationResult Ok() => new(true, null, null);

    /// <summary>
    /// 建立失敗結果（使用 ErrorCode 枚舉）
    /// </summary>
    public static new OperationResult Fail(ErrorCode errorCode) => new(false, errorCode, ErrorMessages.GetMessage(errorCode));

    /// <summary>
    /// 建立失敗結果（使用 ErrorCode 枚舉和自定義訊息）
    /// </summary>
    public static new OperationResult Fail(ErrorCode errorCode, string customMessage) => new(false, errorCode, customMessage);
}
