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
        if (
            httpContext.Items.TryGetValue(
                DeviceFingerprintMiddleware.CONTEXT_KEY,
                out var deviceFingerprint
            )
        )
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
}
