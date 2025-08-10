namespace Monolithic.Shared.Middleware;

/// <summary>
/// Middleware 負責從 HTTP Header 提取 DeviceFingerprint 並存到 HttpContext.Items
/// </summary>
public class DeviceFingerprintMiddleware
{
    private readonly RequestDelegate _next;
    private const string HEADER_NAME = "X-Device-Fingerprint";
    public const string CONTEXT_KEY = "DeviceFingerprint";

    public DeviceFingerprintMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 從 HTTP Header 取得 DeviceFingerprint
        if (context.Request.Headers.TryGetValue(HEADER_NAME, out var deviceFingerprint))
        {
            // 存到 HttpContext.Items，供後續使用
            context.Items[CONTEXT_KEY] = deviceFingerprint.ToString();
        }

        // 繼續執行下一個 middleware
        await _next(context);
    }
}
