using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.Communication;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Extensions;

namespace Monolithic.Features.User.Commands;

public class UpdateNicknameCommand : IRequest
{
    public string NewNickname { get; set; } // 新的暱稱

    public UpdateNicknameCommand(string newNickname)
    {
        NewNickname = newNickname;
    }
}

public class UpdateNicknameCommandHandler : IRequestHandler<UpdateNicknameCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IHubContext<CommunicationHub> _hubContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateNicknameCommandHandler(
        IUserRepository userRepository,
        IHubContext<CommunicationHub> hubContext,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userRepository = userRepository;
        _hubContext = hubContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(UpdateNicknameCommand command, CancellationToken cancellationToken)
    {
        // 取得 DeviceFingerprint
        var deviceFingerprint = _httpContextAccessor.HttpContext?.GetDeviceFingerprint();
        if (string.IsNullOrEmpty(deviceFingerprint))
            throw new UnauthorizedAccessException("無法驗證裝置指紋，請重新登入以繼續操作。");
        if (string.IsNullOrEmpty(command.NewNickname))
            throw new ArgumentException("新暱稱不能為空。");
        var user = await _userRepository.GetByDeviceFingerprintAsync(deviceFingerprint);
        if (user == null)
            throw new InvalidOperationException("找不到對應的使用者，請確認裝置指紋是否正確。");
        var oldNickname = user.Nickname;
        user.Nickname = command.NewNickname;
        await _userRepository.UpdateAsync(user);
        // 廣播給所有用戶（全域通知）
        await _hubContext.Clients.All.SendAsync("NicknameUpdated", user.Id, user.Nickname);
        // TODO: 若只通知同房間，查詢房間後 group 發送
    }
}
