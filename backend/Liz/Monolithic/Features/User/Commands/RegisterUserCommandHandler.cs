using MediatR;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Commands;

/// <summary>
/// 用戶註冊命令處理器
/// </summary>
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IAppLogger<RegisterUserCommandHandler> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("處理用戶註冊命令", new
        {
            request.ExistingUserId,
            request.DeviceFingerprint,
            HasUserAgent = !string.IsNullOrEmpty(request.UserAgent),
            HasIpAddress = !string.IsNullOrEmpty(request.IpAddress)
        });

        try
        {
            // 1. 嘗試重新連線現有用戶
            if (request.ExistingUserId.HasValue)
            {
                var result = await TryReconnectExistingUserAsync(request);
                if (result != null) return result;
            }

            // 2. 嘗試通過設備指紋找到現有用戶
            var existingResult = await TryFindExistingUserAsync(request);
            if (existingResult != null) return existingResult;

            // 3. 創建新用戶
            return await CreateNewUserAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError("用戶註冊失敗", ex, new { request.DeviceFingerprint });
            throw;
        }
    }

    /// <summary>
    /// 嘗試重新連線現有用戶
    /// </summary>
    private async Task<RegisterUserResult?> TryReconnectExistingUserAsync(RegisterUserCommand request)
    {
        var existingUser = await _userRepository.GetByIdAsync(request.ExistingUserId!.Value);
        if (existingUser == null || existingUser.DeviceFingerprint != request.DeviceFingerprint)
        {
            return null;
        }

        existingUser.LastActiveAt = DateTime.UtcNow;
        existingUser.IsActive = true;
        await _userRepository.UpdateAsync(existingUser);

        _logger.LogInfo("用戶重新連線成功", new { UserId = existingUser.Id });

        return CreateUserResult(existingUser, isNewUser: false);
    }

    /// <summary>
    /// 嘗試通過設備指紋找到現有用戶
    /// </summary>
    private async Task<RegisterUserResult?> TryFindExistingUserAsync(RegisterUserCommand request)
    {
        var existingUser = await _userRepository.GetByDeviceFingerprintAsync(request.DeviceFingerprint);
        if (existingUser == null) return null;

        existingUser.LastActiveAt = DateTime.UtcNow;
        existingUser.IsActive = true;
        await _userRepository.UpdateAsync(existingUser);

        _logger.LogInfo("通過設備指紋找到現有用戶", new { UserId = existingUser.Id });

        return CreateUserResult(existingUser, isNewUser: false);
    }

    /// <summary>
    /// 創建新用戶
    /// </summary>
    private async Task<RegisterUserResult> CreateNewUserAsync(RegisterUserCommand request)
    {
        var newUser = new Infrastructure.Data.Entities.User
        {
            Id = Guid.NewGuid(),
            DeviceFingerprint = request.DeviceFingerprint,
            Nickname = GenerateRandomNickname(),
            IsActive = true,
            LastActiveAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(newUser);

        _logger.LogInfo("新用戶創建成功", new { UserId = newUser.Id, Nickname = newUser.Nickname });

        return CreateUserResult(newUser, isNewUser: true);
    }

    /// <summary>
    /// 創建用戶結果對象
    /// </summary>
    private static RegisterUserResult CreateUserResult(Infrastructure.Data.Entities.User user, bool isNewUser)
    {
        return new RegisterUserResult
        {
            UserId = user.Id,
            Nickname = user.Nickname,
            IsNewUser = isNewUser,
            IsActive = user.IsActive,
            LastActiveAt = user.LastActiveAt,
            CreatedAt = user.CreatedAt
        };
    }

    /// <summary>
    /// 生成隨機暱稱
    /// </summary>
    private static string GenerateRandomNickname()
    {
        var random = new Random();
        var number = random.Next(1000, 9999);
        return $"匿名用戶{number}";
    }
}
