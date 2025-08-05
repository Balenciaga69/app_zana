using MediatR;
using Monolithic.Features.Identity.Models;
using Monolithic.Features.Identity.Requests;
using Monolithic.Features.Identity.Services;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Handlers;

/// <summary>
/// 根據 UserId 取得用戶資訊的 Query Handler
/// </summary>
public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserSession?>
{
    private readonly IIdentityService _identityService;
    private readonly IAppLogger<GetUserByIdHandler> _logger;

    public GetUserByIdHandler(IIdentityService identityService, IAppLogger<GetUserByIdHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<UserSession?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理取得用戶資訊請求", new { request.UserId });

        return await _identityService.GetUserByIdAsync(request.UserId);
    }
}
