using FluentValidation;
using MediatR;

namespace Monolithic.Features.User.Commands;

public record UpdateNicknameCommand(Guid UserId, string NewNickname) : IRequest<bool>
{
    // TODO: 更新暱稱的 Command 實作
}

public class UpdateNicknameCommandHandler : IRequestHandler<UpdateNicknameCommand, bool>
{
    public Task<bool> Handle(UpdateNicknameCommand request, CancellationToken cancellationToken)
    {
        // TODO: 更新暱稱的 Handler 實作
        throw new NotImplementedException();
    }
}

public class UpdateNicknameCommandValidator : AbstractValidator<UpdateNicknameCommand>
{
    public UpdateNicknameCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewNickname).NotEmpty().MaximumLength(32); // 可依需求調整長度
    }
}
