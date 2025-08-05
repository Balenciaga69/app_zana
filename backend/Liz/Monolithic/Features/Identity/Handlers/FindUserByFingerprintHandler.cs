using MediatR;
using Monolithic.Features.Identity.Models;
using Monolithic.Features.Identity.Requests;
using Monolithic.Infrastructure.Data;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Handlers;

/// <summary>
/// 依據指紋查找用戶的 Query Handler
/// </summary>
public class FindUserByFingerprintHandler : IRequestHandler<FindUserByFingerprintQuery, UserSession?>
{
    private readonly AppDbContext _context;
    private readonly IAppLogger<FindUserByFingerprintHandler> _logger;

    public FindUserByFingerprintHandler(AppDbContext context, IAppLogger<FindUserByFingerprintHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserSession?> Handle(FindUserByFingerprintQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理依指紋查找用戶請求", new { request.Request.BrowserFingerprint });

        // TODO: 業務邏輯待實作
        await Task.CompletedTask;
        return null;
    }
}
