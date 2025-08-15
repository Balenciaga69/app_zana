using FluentValidation;
using MediatR;

namespace Monolithic.Features.User.Commands;

public record RegisterUserCommand(string? ExistingUserId, string DeviceFingerprint) : IRequest<Guid>
{
    // TODO: 註冊或重新連線用戶的 Command 實作
}

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
        // 若有 existingUserId，建議可加 Guid 格式驗證（視需求）
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
    public Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // TODO: 註冊或重新連線用戶
        // 1. 檢查 deviceFingerprint 是否有效
        // 2. 若 existingUserId 有值則驗證與 deviceFingerprint 綁定關係
        // 3. 若無 existingUserId 則建立新用戶
        // 4. 記錄連線資訊（IP、UserAgent、ConnectionId）
        // 5. 回傳 UserRegistered/ConnectionEstablished 事件
        throw new NotImplementedException();
    }
}
