using MediatR;

namespace Monolithic.Features.Health.Queries.GetHealthStatus;

/// <summary>
/// 取得健康檢查狀態的 Query
/// </summary>
public class GetHealthStatusQuery : IRequest<object>
{
    public bool IncludeDetails { get; set; } = false;
    public string? Tag { get; set; }
    public string? PropertyName { get; set; }

    public GetHealthStatusQuery(bool includeDetails = false)
    {
        IncludeDetails = includeDetails;
    }
}
