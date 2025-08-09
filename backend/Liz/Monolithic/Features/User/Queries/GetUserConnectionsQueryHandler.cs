using MediatR;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Queries;

/// <summary>
/// 查詢用戶連線狀態查詢處理器
/// </summary>
public class GetUserConnectionsQueryHandler : IRequestHandler<GetUserConnectionsQuery, GetUserConnectionsResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger<GetUserConnectionsQueryHandler> _logger;

    public GetUserConnectionsQueryHandler(IUserRepository userRepository, IAppLogger<GetUserConnectionsQueryHandler> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<GetUserConnectionsResult> Handle(GetUserConnectionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(
            "查詢用戶連線歷史",
            new
            {
                request.UserId,
                request.Skip,
                request.Take,
            }
        );

        try
        {
            var connections = await _userRepository.GetUserConnectionsAsync(request.UserId, request.Skip, request.Take);
            var totalCount = await _userRepository.GetUserConnectionsCountAsync(request.UserId);

            _logger.LogInfo(
                "查詢用戶連線歷史完成",
                new
                {
                    request.UserId,
                    ConnectionCount = connections.Count(),
                    totalCount,
                }
            );

            return new GetUserConnectionsResult
            {
                Connections = connections.Select(c => new UserConnectionInfo
                {
                    Id = c.Id,
                    ConnectionId = c.ConnectionId,
                    ConnectedAt = c.ConnectedAt,
                    DisconnectedAt = c.DisconnectedAt,
                    IpAddress = c.IpAddress,
                    UserAgent = c.UserAgent,
                }),
                TotalCount = totalCount,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("查詢用戶連線歷史失敗", ex, new { request.UserId });
            throw;
        }
    }
}
