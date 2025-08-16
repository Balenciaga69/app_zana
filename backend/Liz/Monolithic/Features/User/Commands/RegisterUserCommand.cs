using FluentValidation;
using MediatR;
using Monolithic.Features.User.Repositories;
using Monolithic.Infrastructure.Data;
using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Features.User.Commands;

public record RegisterUserCommand(
    string DeviceFingerprint,
    string ConnectionId,
    string? IpAddress,
    string? UserAgent
) : IRequest<Guid>;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.DeviceFingerprint)
            .NotEmpty()
            .Length(32, 128)
            .Must(f => !string.IsNullOrWhiteSpace(f))
            .WithMessage("DeviceFingerprint 不可為空白")
            .Must(f => !IsAllZeroOrWhitespace(f))
            .WithMessage("DeviceFingerprint 不可全為 0 或全空白")
            .Matches("^[a-zA-Z0-9\\-_.:]+$") // 注意：C# 字串中 - 需跳脫
            .WithMessage("DeviceFingerprint 僅允許英數字與 -_.: 字元");

        RuleFor(x => x.ConnectionId).NotEmpty().WithMessage("ConnectionId 不可為空");
        // IpAddress, UserAgent 可選，不強制驗證
    }

    private bool IsAllZeroOrWhitespace(string f)
    {
        if (string.IsNullOrWhiteSpace(f))
        {
            return true;
        }
        var trimmed = f.Trim();
        return trimmed.All(c => c == '0');
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserConnectionRepository _userConnectionRepository;
    private readonly AppDbContext _dbContext;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUserConnectionRepository userConnectionRepository,
        AppDbContext dbContext
    )
    {
        _userRepository = userRepository;
        _userConnectionRepository = userConnectionRepository;
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // 查找是否已有對應 DeviceFingerprint 的 User
        var existingUser = await _userRepository.GetByDeviceFingerprintAsync(request.DeviceFingerprint);

        UserEntity user;

        if (existingUser == null)
        {
            // 若無則建立新的 User
            user = new UserEntity
            {
                Id = Guid.NewGuid(),
                DeviceFingerprint = request.DeviceFingerprint,
                IsActive = true,
                LastActiveAt = now,
                Nickname = $"匿名用戶{Random.Shared.Next(1000, 9999)}",
                CreatedAt = now,
                UpdatedAt = now,
            };

            await _userRepository.AddAsync(user);
        }
        else
        {
            // 若有則更新 IsActive 和 LastActiveAt
            existingUser.IsActive = true;
            existingUser.LastActiveAt = now;
            existingUser.UpdatedAt = now;

            await _userRepository.UpdateAsync(existingUser);
            user = existingUser;
        }

        // 建立一筆 UserConnection
        var userConnection = new UserConnectionEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id.ToString(),
            ConnectionId = request.ConnectionId,
            ConnectedAt = now,
            DisconnectedAt = null,
            IpAddress = request.IpAddress,
            UserAgent = request.UserAgent,
            CreatedAt = now,
            UpdatedAt = now,
        };

        await _userConnectionRepository.AddAsync(userConnection);

        // 5. 儲存變更
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
