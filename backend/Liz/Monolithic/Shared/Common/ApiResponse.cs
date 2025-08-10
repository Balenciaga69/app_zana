namespace Monolithic.Shared.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Code { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? TraceId { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "OK") =>
        new ApiResponse<T>
        {
            Success = true,
            Code = "OK",
            Message = message,
            Data = data,
        };

    public static ApiResponse<T> Fail(string code, string message, object? errors = null) =>
        new ApiResponse<T>
        {
            Success = false,
            Code = code,
            Message = message,
            Errors = errors,
        };

    public static ApiResponse<T> Fail(ErrorCode errorCode, object? errors = null) =>
        new ApiResponse<T>
        {
            Success = false,
            Code = errorCode.ToString(),
            Message = ErrorMessages.GetMessage(errorCode),
            Errors = errors,
        };

    public static ApiResponse<T> Fail(ErrorCode errorCode, string customMessage, object? errors = null) =>
        new ApiResponse<T>
        {
            Success = false,
            Code = errorCode.ToString(),
            Message = customMessage,
            Errors = errors,
        };
}
