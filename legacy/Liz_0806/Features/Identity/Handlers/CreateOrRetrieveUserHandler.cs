using MediatR;
using Monolithic.Features.Identity.Models;
using Monolithic.Features.Identity.Requests;
using Monolithic.Infrastructure.Data;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Handlers;

/// <summary>
/// 建立或找回用戶的 Command Handler
/// </summary>
public class CreateOrRetrieveUserHandler : IRequestHandler<CreateOrRetrieveUserCommand, UserSession>
{
    private readonly AppDbContext _context;
    private readonly IAppLogger<CreateOrRetrieveUserHandler> _logger;

    public CreateOrRetrieveUserHandler(AppDbContext context, IAppLogger<CreateOrRetrieveUserHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserSession> Handle(CreateOrRetrieveUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理建立或找回用戶請求", new { request.BrowserFingerprint });

        // TODO: 業務邏輯待實作
        await Task.CompletedTask;

        // 暫時回傳空 UserSession
        return new UserSession();
    }
}
