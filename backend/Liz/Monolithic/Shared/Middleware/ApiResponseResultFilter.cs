using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monolithic.Shared.Common;

namespace Monolithic.Shared.Middleware
{
    public class ApiResponseResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value is not ApiResponse<object> &&
                objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
            {
                var valueType = objectResult.Value?.GetType() ?? typeof(object);
                var apiResponseType = typeof(ApiResponse<>).MakeGenericType(valueType);
                var wrapped = Activator.CreateInstance(apiResponseType);
                if (wrapped != null)
                {
                    apiResponseType.GetProperty("Success")?.SetValue(wrapped, true);
                    apiResponseType.GetProperty("Code")?.SetValue(wrapped, "OK");
                    apiResponseType.GetProperty("Message")?.SetValue(wrapped, "OK");
                    apiResponseType.GetProperty("Data")?.SetValue(wrapped, objectResult.Value);
                    apiResponseType.GetProperty("Timestamp")?.SetValue(wrapped, DateTime.UtcNow);
                    // 取得 TraceId
                    var traceId = context.HttpContext.TraceIdentifier;
                    apiResponseType.GetProperty("TraceId")?.SetValue(wrapped, traceId);
                    context.Result = new ObjectResult(wrapped)
                    {
                        StatusCode = objectResult.StatusCode
                    };
                }
            }
            await next();
        }
    }
}
