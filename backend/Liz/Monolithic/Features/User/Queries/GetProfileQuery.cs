using MediatR;
using Monolithic.Features.User.Repositories;

namespace Monolithic.Features.User.Queries;

public record GetProfileQuery(Guid UserId) : IRequest<GetProfileResult>;

public class GetProfileResult
{
    public string Nickname { get; set; } = string.Empty;
    // 未來可擴充更多欄位
}

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, GetProfileResult>
{
    private readonly IUserRepository _userRepository;

    public GetProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetProfileResult> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        return user == null ? new GetProfileResult() : new GetProfileResult { Nickname = user.Nickname };
    }
}
