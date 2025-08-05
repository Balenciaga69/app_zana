using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Monolithic.Features.Health.Requests;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Health.Handlers;

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

    public async Task<object> Handle(GetHealthStatusQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理健康檢查狀態請求", new { request.IncludeDetails, request.Tag, request.PropertyName });

        // 標籤健康檢查
        if (!string.IsNullOrWhiteSpace(request.Tag) && !string.IsNullOrWhiteSpace(request.PropertyName))
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(check => check.Tags.Contains(request.Tag), cancellationToken);
            var taggedChecks = healthReport.Entries.Where(e => e.Value.Tags.Contains(request.Tag));

            var response = new Dictionary<string, object>
            {
                ["status"] = healthReport.Status.ToString(),
                ["timestamp"] = DateTime.UtcNow,
                [request.PropertyName] = taggedChecks.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    duration = entry.Value.Duration.TotalMilliseconds,
                    description = entry.Value.Description,
                })
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
                entries = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    duration = entry.Value.Duration.TotalMilliseconds,
                    description = entry.Value.Description,
                    exception = entry.Value.Exception?.Message,
                    tags = entry.Value.Tags,
                }),
            };
        }
        return new { status = report.Status.ToString(), timestamp = DateTime.UtcNow };
    }
}
