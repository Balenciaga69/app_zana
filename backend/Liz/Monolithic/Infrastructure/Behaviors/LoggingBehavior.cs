using MediatR;
using Monolithic.Shared.Logging;
using System.Diagnostics;

namespace Infrastructure.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IAppLogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(IAppLogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var requestName = typeof(TRequest).Name;
        var traceId = Guid.NewGuid().ToString("N").Substring(0, 8);
        _logger.LogInfo($"Handling {requestName}", request, traceId);
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await next(cancellationToken);
            stopwatch.Stop();
            _logger.LogInfo($"Handled {requestName} in {stopwatch.ElapsedMilliseconds}ms", response, traceId);
            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(
                $"Error handling {requestName} after {stopwatch.ElapsedMilliseconds}ms",
                ex,
                request,
                traceId
            );
            throw;
        }
    }
}
