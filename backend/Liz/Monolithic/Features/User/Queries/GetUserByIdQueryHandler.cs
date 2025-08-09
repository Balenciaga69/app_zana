using MediatR;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Queries;

/// <summary>
/// 根據 ID 查找用戶查詢處理器
/// </summary>
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResult?>
{
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IAppLogger<GetUserByIdQueryHandler> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }    public async Task<GetUserByIdResult?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("開始查詢用戶通過 ID", new { request.UserId });

        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                _logger.LogInfo("未找到指定用戶", new { request.UserId });
                return null;
            }

            _logger.LogInfo("找到指定用戶", new { UserId = user.Id });

            return new GetUserByIdResult
            {
                UserId = user.Id,
                Nickname = user.Nickname,
                IsActive = user.IsActive,
                LastActiveAt = user.LastActiveAt,
                CreatedAt = user.CreatedAt,
                DeviceFingerprint = user.DeviceFingerprint,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("查詢用戶失敗", ex, new { request.UserId });
            throw;
        }
    }
}
