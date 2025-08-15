using FluentValidation;
using MediatR;

namespace Monolithic.Features.User.Commands;

public record UpdateNicknameCommand(Guid UserId, string NewNickname) : IRequest<bool>
{
    // TODO: 更新暱稱的 Command 實作
}

public class UpdateNicknameCommandValidator : AbstractValidator<UpdateNicknameCommand>
{
    public UpdateNicknameCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewNickname).NotEmpty().MaximumLength(32); // 可依需求調整長度
    }
}

public class UpdateNicknameCommandHandler : IRequestHandler<UpdateNicknameCommand, bool>
{
    public Task<bool> Handle(UpdateNicknameCommand request, CancellationToken cancellationToken)
    {
        // TODO: 即時更新暱稱
        // 1. 驗證 newNickname 格式與長度
        // 2. 取得當前 userId
        // 3. 更新資料庫/快取中的暱稱
        // 4. 廣播 NicknameUpdated 事件給相關用戶
        throw new NotImplementedException();
    }
}
