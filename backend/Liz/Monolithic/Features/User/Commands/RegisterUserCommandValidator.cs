using FluentValidation;
using Monolithic.Features.User.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.DeviceFingerprint).NotEmpty();
        // 若有 existingUserId，可加 Guid 格式驗證（視需求）
    }
}
