using MediatR;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Extensions;

namespace Monolithic.Features.User.Queries;

public class GetMeQuery : IRequest<GetMeResult> { }

public class GetMeResult
{
    public Guid Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string DeviceFingerprint { get; set; } = string.Empty;
    public DateTime LastActiveAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, GetMeResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetMeQueryHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetMeResult> Handle(GetMeQuery query, CancellationToken cancellationToken)
    {
        // 透過 HttpContext 擴充方法取得 DeviceFingerprint
        var deviceFingerprint = _httpContextAccessor.HttpContext?.GetDeviceFingerprint();

        // 沒有 DeviceFingerprint -> 丟出例外
        if (string.IsNullOrEmpty(deviceFingerprint))
            throw new UnauthorizedAccessException("Device fingerprint is required");

        // 查詢 User
        var user = await _userRepository.GetByDeviceFingerprintAsync(deviceFingerprint);

        // 處理找不到用戶 -> InvalidOperationException
        if (user == null)
            throw new InvalidOperationException("User not found");

        // 組裝 GetMeResult 回傳
        return new GetMeResult
        {
            Id = user.Id,
            Nickname = user.Nickname,
            DeviceFingerprint = user.DeviceFingerprint,
            LastActiveAt = user.LastActiveAt,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
        };
    }
}
