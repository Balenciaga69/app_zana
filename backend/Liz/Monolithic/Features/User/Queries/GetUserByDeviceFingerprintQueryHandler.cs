using MediatR;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Queries;

/// <summary>
/// 根據設備指紋查找用戶查詢處理器
/// </summary>
public class GetUserByDeviceFingerprintQueryHandler
    : IRequestHandler<GetUserByDeviceFingerprintQuery, GetUserByDeviceFingerprintResult?>
{
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger<GetUserByDeviceFingerprintQueryHandler> _logger;

    public GetUserByDeviceFingerprintQueryHandler(
        IUserRepository userRepository,
        IAppLogger<GetUserByDeviceFingerprintQueryHandler> logger
    )
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<GetUserByDeviceFingerprintResult?> Handle(
        GetUserByDeviceFingerprintQuery request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInfo("開始查詢用戶通過設備指紋", new { request.DeviceFingerprint });

        try
        {
            var user = await _userRepository.GetByDeviceFingerprintAsync(request.DeviceFingerprint);

            if (user == null)
            {
                _logger.LogInfo("未找到匹配的用戶", new { request.DeviceFingerprint });
                return null;
            }

            _logger.LogInfo("找到匹配的用戶", new { UserId = user.Id, request.DeviceFingerprint });

            return new GetUserByDeviceFingerprintResult
            {
                UserId = user.Id,
                Nickname = user.Nickname,
                IsActive = user.IsActive,
                LastActiveAt = user.LastActiveAt,
                CreatedAt = user.CreatedAt,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("查詢用戶失敗", ex, new { request.DeviceFingerprint });
            throw;
        }
    }
}
