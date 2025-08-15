using MediatR;
using FluentValidation;

namespace Monolithic.Features.User.Commands;

public record RegisterUserCommand(string? ExistingUserId, string DeviceFingerprint) : IRequest<Guid>
{
    // TODO: 註冊或重新連線用戶的 Command 實作
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    public Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // TODO: 註冊或重新連線用戶的 Handler 實作
        throw new NotImplementedException();
    }
}

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.DeviceFingerprint).NotEmpty();
        // 若有 existingUserId，建議可加 Guid 格式驗證（視需求）
    }
}
