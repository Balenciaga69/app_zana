using FluentValidation;
using Monolithic.Features.User.Commands;

public class UpdateNicknameCommandValidator : AbstractValidator<UpdateNicknameCommand>
{
    public UpdateNicknameCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewNickname).NotEmpty().MaximumLength(32); // 可依需求調整長度
    }
}
