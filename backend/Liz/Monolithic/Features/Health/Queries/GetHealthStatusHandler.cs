using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Health.Queries;

/// <summary>
/// 取得健康檢查狀態的 Query Handler
/// </summary>
public class GetHealthStatusHandler : IRequestHandler<GetHealthStatusQuery, object>
{
    private readonly HealthCheckService _healthCheckService;
    private readonly IAppLogger<GetHealthStatusHandler> _logger;

    public GetHealthStatusHandler(HealthCheckService healthCheckService, IAppLogger<GetHealthStatusHandler> logger)
    {
        _healthCheckService = healthCheckService;
        _logger = logger;
    }

    private IEnumerable<object> MapHealthEntries(IEnumerable<KeyValuePair<string, HealthReportEntry>> entries, bool includeExceptionAndTags = false)
    {
        // 如果需要包含例外和標籤
        if (includeExceptionAndTags)
        {
            return entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                duration = entry.Value.Duration.TotalMilliseconds,
                description = entry.Value.Description,
                exception = entry.Value.Exception?.Message,
                tags = entry.Value.Tags,
            });
        }
        else
        {
            return entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                duration = entry.Value.Duration.TotalMilliseconds,
                description = entry.Value.Description,
            });
        }
    }

    public async Task<object> Handle(GetHealthStatusQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(
            "處理健康檢查狀態請求",
            new
            {
                request.IncludeDetails,
                request.Tag,
                request.PropertyName,
            }
        );

        // 標籤健康檢查
        if (!string.IsNullOrWhiteSpace(request.Tag) && !string.IsNullOrWhiteSpace(request.PropertyName))
        {
            // 檢查是否有指定標籤
            var healthReport = await _healthCheckService.CheckHealthAsync(check => check.Tags.Contains(request.Tag), cancellationToken);
            // 如果有指定標籤，則返回該標籤的健康檢查結果
            var taggedChecks = healthReport.Entries.Where(e => e.Value.Tags.Contains(request.Tag));

            var response = new Dictionary<string, object>
            {
                ["status"] = healthReport.Status.ToString(),
                ["timestamp"] = DateTime.UtcNow,
                [request.PropertyName] = MapHealthEntries(taggedChecks),
            };
            return response;
        }

        // 一般健康檢查
        var report = await _healthCheckService.CheckHealthAsync(cancellationToken);
        if (request.IncludeDetails)
        {
            return new
            {
                status = report.Status.ToString(),
                totalDuration = report.TotalDuration.TotalMilliseconds,
                timestamp = DateTime.UtcNow,
                entries = MapHealthEntries(report.Entries, true),
            };
        }
        return new { status = report.Status.ToString(), timestamp = DateTime.UtcNow };
    }
}
