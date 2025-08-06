using MediatR;
using Monolithic.Features.Identity.Requests;
using Monolithic.Infrastructure.Data;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Handlers;

/// <summary>
/// 設定用戶上線狀態的 Command Handler
/// </summary>
public class SetUserOnlineStatusHandler : IRequestHandler<SetUserOnlineStatusCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly IAppLogger<SetUserOnlineStatusHandler> _logger;

    public SetUserOnlineStatusHandler(AppDbContext context, IAppLogger<SetUserOnlineStatusHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(SetUserOnlineStatusCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理設定用戶上線狀態請求", new { request.UserId, request.IsOnline });

        // TODO: 業務邏輯待實作
        await Task.CompletedTask;
        return Unit.Value;
    }
}
