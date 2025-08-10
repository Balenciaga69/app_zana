using Monolithic.Shared.Middleware;

namespace Monolithic.Shared.Extensions;

/// <summary>
/// HttpContext 擴充方法，方便取得 DeviceFingerprint
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// 從 HttpContext.Items 取得 DeviceFingerprint
    /// </summary>
    public static string? GetDeviceFingerprint(this HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue(DeviceFingerprintMiddleware.CONTEXT_KEY, out var deviceFingerprint))
        {
            return deviceFingerprint?.ToString();
        }
        return null;
    }

    /// <summary>
    /// 檢查是否有 DeviceFingerprint
    /// </summary>
    public static bool HasDeviceFingerprint(this HttpContext httpContext)
    {
        return !string.IsNullOrEmpty(httpContext.GetDeviceFingerprint());
    }

    /// <summary>
    /// 取得用戶 IP Address
    /// </summary>
    public static string? GetIpAddress(this HttpContext httpContext)
    {
        return httpContext?.Connection?.RemoteIpAddress?.ToString();
    }

    /// <summary>
    /// 取得 UserAgent
    /// </summary>
    public static string? GetUserAgent(this HttpContext httpContext)
    {
        return httpContext?.Request?.Headers["User-Agent"].ToString();
    }
}
