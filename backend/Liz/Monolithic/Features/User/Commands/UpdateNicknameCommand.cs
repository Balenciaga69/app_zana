using FluentValidation;
using MediatR;
using Monolithic.Features.User.Repositories;
using Monolithic.Infrastructure.Data;

namespace Monolithic.Features.User.Commands;

public record UpdateNicknameCommand(Guid UserId, string NewNickname) : IRequest<bool>;

public class UpdateNicknameCommandValidator : AbstractValidator<UpdateNicknameCommand>
{
    public UpdateNicknameCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User not registered");

        RuleFor(x => x.NewNickname)
            .NotEmpty()
            .WithMessage("Nickname cannot be empty")
            .MaximumLength(32)
            .WithMessage("Nickname cannot exceed 32 characters");
    }
}

public class UpdateNicknameCommandHandler : IRequestHandler<UpdateNicknameCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _db;

    public UpdateNicknameCommandHandler(IUserRepository userRepository, AppDbContext db)
    {
        _userRepository = userRepository;
        _db = db;
    }

    public async Task<bool> Handle(UpdateNicknameCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var newNickname = request.NewNickname;

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        // 更新暱稱和時間戳
        user.Nickname = newNickname;
        user.UpdatedAt = DateTime.UtcNow;

        // 儲存變更
        await _userRepository.UpdateAsync(user);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
