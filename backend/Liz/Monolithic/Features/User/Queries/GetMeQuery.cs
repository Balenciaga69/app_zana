using MediatR;
using Microsoft.AspNetCore.Http;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Extensions;

namespace Monolithic.Features.User.Queries;

// CQRS: Command (這裡其實是 Query)
public class GetMeQuery
{
    // 這裡可加上必要的屬性，例如用戶識別資訊（如 UserId, DeviceFingerprint, HttpContext 等）
    // 目前假設由 Handler 取得當前用戶資訊，這裡不需要額外屬性
}

// CQRS: Result (回傳型別)
public class GetMeResult
{
    public Guid Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string DeviceFingerprint { get; set; } = string.Empty;
    public DateTime LastActiveAt { get; set; }
}

// CQRS: Handler
public class GetMeQueryHandler : IRequestHandler<GetMeQuery, GetMeResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetMeQueryHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    // public async Task<GetMeResult> Handle(GetMeQuery query, CancellationToken cancellationToken)
    // {
    //     // 1. 透過 HttpContext 擴充方法取得 DeviceFingerprint
    //     // var deviceFingerprint = _httpContextAccessor.HttpContext?.GetDeviceFingerprint();
    //     // 2. 若沒有 DeviceFingerprint，可丟例外或回傳錯誤
    //     // 3. 用 _userRepository.GetByDeviceFingerprintAsync 查詢 User
    //     // 4. 組裝 GetMeResult 回傳
    //     // 5. 處理找不到用戶的情境（可丟例外或回傳 null）
    // }
}
