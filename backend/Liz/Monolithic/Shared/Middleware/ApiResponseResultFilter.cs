using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monolithic.Shared.Common;

namespace Monolithic.Shared.Middleware
{
    // API 回應結果過濾器
    public class ApiResponseResultFilter : IAsyncResultFilter
    {
        private const bool DefaultSuccess = true;
        private const string DefaultCode = "OK";
        private const string DefaultMessage = "OK";

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // 檢查回傳結果是否為 ObjectResult，且不是 ApiResponse，且狀態碼為 2xx
            if (
                context.Result is ObjectResult objectResult // 判斷 context.Result 是否為 ObjectResult 型別
                && !IsApiResponse(objectResult.Value) // 判斷結果值是否已經是 ApiResponse 型別
                && objectResult.StatusCode is >= 200 and < 300 // 判斷 HTTP 狀態碼是否為 2xx（成功）
            )
            {
                // 取得回傳值的型別，若為 null 則使用 object
                var valueType = objectResult.Value?.GetType() ?? typeof(object);
                // 建立泛型 ApiResponse<T> 型別
                var apiResponseType = typeof(ApiResponse<>).MakeGenericType(valueType);
                // 透過反射建立 ApiResponse<T> 實例
                var wrapped = Activator.CreateInstance(apiResponseType);
                if (wrapped != null)
                {
                    // 建立要設定到 ApiResponse 的屬性與值
                    var propertyValues = new Dictionary<string, object?>
                    {
                        ["Success"] = DefaultSuccess, // 設定成功屬性
                        ["Code"] = DefaultCode, // 設定回應代碼
                        ["Message"] = DefaultMessage, // 設定訊息
                        ["Data"] = objectResult.Value, // 設定資料內容
                        ["Timestamp"] = DateTime.UtcNow, // 設定時間戳
                        ["TraceId"] = context.HttpContext.TraceIdentifier, // 設定追蹤 ID
                    };
                    // 逐一設定 ApiResponse 屬性值
                    foreach (var kvp in propertyValues)
                    {
                        apiResponseType.GetProperty(kvp.Key)?.SetValue(wrapped, kvp.Value);
                    }
                    // 將包裝後的 ApiResponse 設為新的回傳結果
                    context.Result = new ObjectResult(wrapped) { StatusCode = objectResult.StatusCode };
                }
            }
            // 執行下一個過濾器或動作
            await next();
        }

        // 檢查是否已經是 ApiResponse 的實例
        private static bool IsApiResponse(object? value)
        {
            return value is not null && value.GetType() is { IsGenericType: true } type && type.GetGenericTypeDefinition() == typeof(ApiResponse<>);
        }
    }
}
