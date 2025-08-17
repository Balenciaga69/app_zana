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
            .Matches("^[a-zA-Z0-9\\-_.:]+$") // 允許英數字與 -_.: 字元
            .WithMessage("DeviceFingerprint 僅允許英數字與 -_.: 字元");

        RuleFor(x => x.ConnectionId).NotEmpty().WithMessage("ConnectionId 不可為空");
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

        // 撈取或建立 User
        var existingUser = await _userRepository.GetByDeviceFingerprintAsync(request.DeviceFingerprint);

        UserEntity user;

        if (existingUser == null)
        {
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
            existingUser.IsActive = true;
            existingUser.LastActiveAt = now;
            existingUser.UpdatedAt = now;

            await _userRepository.UpdateAsync(existingUser);
            user = existingUser;
        }

        // 生成一筆新的 UserConnectionEntity
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

        await _dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
