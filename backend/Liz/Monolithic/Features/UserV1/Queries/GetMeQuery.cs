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
        // 取得 DeviceFingerprint
        var deviceFingerprint = _httpContextAccessor.HttpContext?.GetDeviceFingerprint();

        // 沒有 DeviceFingerprint -> 丟出例外
        if (string.IsNullOrEmpty(deviceFingerprint))
            throw new UnauthorizedAccessException("無法驗證裝置指紋，請重新登入以繼續操作。");

        // 查詢 User
        var user = await _userRepository.GetByDeviceFingerprintAsync(deviceFingerprint);

        // 處理找不到用戶 -> 丟出例外
        if (user == null)
            throw new InvalidOperationException(
                "找不到對應的使用者，請確認裝置指紋是否正確或聯繫系統管理員。"
            );

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
