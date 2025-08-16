using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Monolithic.Features.User.Commands;

public record RegisterUserCommand(string DeviceFingerprint) : IRequest<Guid>
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
    public RegisterUserCommandHandler()
    {
        // TODO: 若需要，注入 repository / dbcontext / logger 等
    }

    public Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 先使用 validator 驗證輸入，若驗證失敗會拋出 ValidationException
        var validator = new RegisterUserCommandValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // TODO: 以下為業務實作要點 - 保留為 TODO（由你要求 persistence 層暫不實作）
        // 1. 使用 repository 查找是否已有對應 DeviceFingerprint 的 User
        // 2. 若無則建立新的 User（設定 IsActive, CreatedAt, Nickname 等）
        // 3. 若有則更新 IsActive = true, LastActiveAt = DateTime.UtcNow
        // 4. 建立一筆 UserConnection（ConnectionId, IpAddress, UserAgent, ConnectedAt）
        // 5. Persist changes (Repository/DbContext.SaveChanges)

        // 為了讓流程可測試且不會中斷，暫時回傳一個新的 Guid
        var userId = Guid.NewGuid();
        return Task.FromResult(userId);
    }
}
