using MediatR;
using Monolithic.Features.Identity.Requests;
using Monolithic.Infrastructure.Data;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Handlers;

/// <summary>
/// 更新用戶活動時間的 Command Handler
/// </summary>
public class UpdateUserActivityHandler : IRequestHandler<UpdateUserActivityCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly IAppLogger<UpdateUserActivityHandler> _logger;

    public UpdateUserActivityHandler(AppDbContext context, IAppLogger<UpdateUserActivityHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateUserActivityCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理更新用戶活動時間請求", new { request.UserId });

        // TODO: 業務邏輯待實作
        await Task.CompletedTask;
        return Unit.Value;
    }
}
