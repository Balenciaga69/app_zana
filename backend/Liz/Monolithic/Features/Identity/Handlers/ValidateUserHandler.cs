using MediatR;
using Monolithic.Features.Identity.Requests;
using Monolithic.Infrastructure.Data;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Handlers;

/// <summary>
/// 驗證用戶身份的 Command Handler
/// </summary>
public class ValidateUserHandler : IRequestHandler<ValidateUserCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly IAppLogger<ValidateUserHandler> _logger;

    public ValidateUserHandler(AppDbContext context, IAppLogger<ValidateUserHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理驗證用戶身份請求", new { request.Request.UserId, request.Request.BrowserFingerprint });

        // TODO: 業務邏輯待實作
        await Task.CompletedTask;
        return false;
    }
}
