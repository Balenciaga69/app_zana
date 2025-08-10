using MediatR;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Common;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Commands;

/// <summary>
/// 更新用戶暱稱命令處理器
/// </summary>
public class UpdateUserNicknameCommandHandler
    : IRequestHandler<UpdateUserNicknameCommand, OperationResult<UpdateUserNicknameResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger<UpdateUserNicknameCommandHandler> _logger;

    public UpdateUserNicknameCommandHandler(
        IUserRepository userRepository,
        IAppLogger<UpdateUserNicknameCommandHandler> logger
    )
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<OperationResult<UpdateUserNicknameResult>> Handle(
        UpdateUserNicknameCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInfo("開始處理更新用戶暱稱請求", new { request.UserId, request.Nickname });
        try
        {
            // 第一步：驗證命令
            var validationError = request.GetValidationError();
            if (validationError != null)
            {
                _logger.LogWarn(
                    "暱稱格式驗證失敗",
                    new
                    {
                        request.UserId,
                        request.Nickname,
                        validationError,
                    }
                );
                return OperationResult<UpdateUserNicknameResult>.Fail(validationError.Value);
            }

            // 第二步：查找用戶
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarn("找不到指定用戶", new { request.UserId });
                return OperationResult<UpdateUserNicknameResult>.Fail(ErrorCode.UserNotFound);
            }

            // 第三步：更新用戶資訊
            var oldNickname = user.Nickname;
            user.Nickname = request.Nickname;
            user.LastActiveAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            _logger.LogInfo(
                "用戶暱稱更新成功",
                new
                {
                    request.UserId,
                    OldNickname = oldNickname,
                    NewNickname = request.Nickname,
                }
            );

            // 第四步：建立結果
            var result = new UpdateUserNicknameResult(user.Id, user.Nickname, user.UpdatedAt);
            return OperationResult<UpdateUserNicknameResult>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("更新用戶暱稱失敗", ex, new { request.UserId, request.Nickname });
            return OperationResult<UpdateUserNicknameResult>.Fail(ErrorCode.InternalServerError);
        }
    }
}
