using MediatR;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Commands;

/// <summary>
/// 更新用戶暱稱命令處理器
/// </summary>
public class UpdateUserNicknameCommandHandler : IRequestHandler<UpdateUserNicknameCommand, UpdateUserNicknameResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger<UpdateUserNicknameCommandHandler> _logger;

    public UpdateUserNicknameCommandHandler(
        IUserRepository userRepository,
        IAppLogger<UpdateUserNicknameCommandHandler> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UpdateUserNicknameResult> Handle(UpdateUserNicknameCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("更新用戶暱稱", new { request.UserId, request.Nickname });

        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarn("用戶不存在", new { request.UserId });
                return new UpdateUserNicknameResult
                {
                    UserId = request.UserId,
                    Nickname = request.Nickname,
                    Success = false,
                    UpdatedAt = DateTime.UtcNow
                };
            }

            // 驗證暱稱格式
            if (string.IsNullOrWhiteSpace(request.Nickname) || request.Nickname.Length > 32)
            {
                _logger.LogWarn("暱稱格式無效", new { request.UserId, request.Nickname });
                return new UpdateUserNicknameResult
                {
                    UserId = request.UserId,
                    Nickname = request.Nickname,
                    Success = false,
                    UpdatedAt = DateTime.UtcNow
                };
            }

            // 更新暱稱
            user.Nickname = request.Nickname;
            user.LastActiveAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            _logger.LogInfo("用戶暱稱更新成功", new { request.UserId, OldNickname = user.Nickname, NewNickname = request.Nickname });

            return new UpdateUserNicknameResult
            {
                UserId = user.Id,
                Nickname = user.Nickname,
                Success = true,
                UpdatedAt = user.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("更新用戶暱稱失敗", ex, new { request.UserId, request.Nickname });
            throw;
        }
    }
}
