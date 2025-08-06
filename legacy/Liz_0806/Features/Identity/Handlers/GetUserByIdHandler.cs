using MediatR;
using Monolithic.Features.Identity.Models;
using Monolithic.Features.Identity.Requests;
using Monolithic.Infrastructure.Data;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Handlers;

/// <summary>
/// 根據 UserId 取得用戶資訊的 Query Handler
/// </summary>
public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserSession?>
{
    private readonly AppDbContext _context;
    private readonly IAppLogger<GetUserByIdHandler> _logger;

    public GetUserByIdHandler(AppDbContext context, IAppLogger<GetUserByIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserSession?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理取得用戶資訊請求", new { request.UserId });

        // TODO: 業務邏輯待實作
        await Task.CompletedTask;
        return null;
    }
}
