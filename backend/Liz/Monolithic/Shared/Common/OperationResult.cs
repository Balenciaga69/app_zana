namespace Monolithic.Shared.Common;

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

    public static OperationResult<T> Ok(T data) => new(true, data, null, null);

    public static OperationResult<T> Fail(ErrorCode errorCode) =>
        new(false, default, errorCode, ErrorMessages.GetMessage(errorCode));

    public static OperationResult<T> Fail(ErrorCode errorCode, string customMessage) =>
        new(false, default, errorCode, customMessage);

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
                return ApiResponse<T>.Fail("Unknown", ErrorMessage ?? "未知錯誤");
            }
        }
    }
}

public class OperationResult : OperationResult<object>
{
    private OperationResult(bool success, ErrorCode? errorCode, string? errorMessage)
        : base(success, null, errorCode, errorMessage) { }

    public static OperationResult Ok() => new(true, null, null);

    public static new OperationResult Fail(ErrorCode errorCode) =>
        new(false, errorCode, ErrorMessages.GetMessage(errorCode));

    public static new OperationResult Fail(ErrorCode errorCode, string customMessage) =>
        new(false, errorCode, customMessage);
}
