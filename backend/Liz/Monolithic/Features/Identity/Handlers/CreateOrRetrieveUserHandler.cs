using MediatR;
using Monolithic.Features.Identity.Models;
using Monolithic.Features.Identity.Requests;
using Monolithic.Features.Identity.Services;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Handlers;

/// <summary>
/// 建立或找回用戶的 Command Handler
/// </summary>
public class CreateOrRetrieveUserHandler : IRequestHandler<CreateOrRetrieveUserCommand, UserSession>
{
    private readonly IIdentityService _identityService;
    private readonly IAppLogger<CreateOrRetrieveUserHandler> _logger;

    public CreateOrRetrieveUserHandler(IIdentityService identityService, IAppLogger<CreateOrRetrieveUserHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<UserSession> Handle(CreateOrRetrieveUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理建立或找回用戶請求", new { request.BrowserFingerprint, request.IpAddress });

        var createUserRequest = new CreateUserRequest
        {
            BrowserFingerprint = request.BrowserFingerprint,
            UserAgent = request.UserAgent,
            IpAddress = request.IpAddress,
            DeviceType = request.DeviceType,
            OperatingSystem = request.OperatingSystem,
            Browser = request.Browser,
            BrowserVersion = request.BrowserVersion,
            Platform = request.Platform,
        };

        return await _identityService.CreateOrRetrieveUserAsync(createUserRequest);
    }
}
