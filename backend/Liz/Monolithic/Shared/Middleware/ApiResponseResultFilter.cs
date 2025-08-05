using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monolithic.Shared.Common;

namespace Monolithic.Shared.Middleware
{
    // API 回應結果過濾器
    public class ApiResponseResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (
                context.Result is ObjectResult objectResult
                && !IsApiResponse(objectResult.Value)
                && objectResult.StatusCode >= 200
                && objectResult.StatusCode < 300
            )
            {
                // 包裝 ObjectResult 的值為 ApiResponse
                var valueType = objectResult.Value?.GetType() ?? typeof(object);
                // 取得值的類型，並建立 ApiResponse 的實例
                var apiResponseType = typeof(ApiResponse<>).MakeGenericType(valueType);
                // 使用反射建立 ApiResponse 的實例
                var wrapped = Activator.CreateInstance(apiResponseType);
                if (wrapped != null)
                {
                    // 設置 ApiResponse 的屬性
                    apiResponseType.GetProperty("Success")?.SetValue(wrapped, true);
                    apiResponseType.GetProperty("Code")?.SetValue(wrapped, "OK");
                    apiResponseType.GetProperty("Message")?.SetValue(wrapped, "OK");
                    apiResponseType.GetProperty("Data")?.SetValue(wrapped, objectResult.Value);
                    apiResponseType.GetProperty("Timestamp")?.SetValue(wrapped, DateTime.UtcNow);
                    // 取得 TraceId
                    var traceId = context.HttpContext.TraceIdentifier;
                    apiResponseType.GetProperty("TraceId")?.SetValue(wrapped, traceId);
                    context.Result = new ObjectResult(wrapped) { StatusCode = objectResult.StatusCode };
                }
            }
            await next();
        }

        // 檢查是否已經是 ApiResponse 的實例
        private static bool IsApiResponse(object? value)
        {
            if (value == null)
            {
                return false;
            }

            // 取得值的類型
            var type = value.GetType();
            // 檢查是否為 ApiResponse<T> 的實例
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ApiResponse<>);
        }
    }
}
