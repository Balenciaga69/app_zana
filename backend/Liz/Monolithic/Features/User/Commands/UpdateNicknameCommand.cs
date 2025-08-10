using Monolithic.Features.User.Services;
using Monolithic.Shared.Extensions;

namespace Monolithic.Features.User.Commands;

public class UpdateNicknameCommand
{
    public string NewNickname { get; set; } // 新的暱稱

    public UpdateNicknameCommand(string newNickname)
    {
        NewNickname = newNickname;
    }
}

public class UpdateNicknameCommandHandler
{
    private readonly IUserCommunicationService _userCommunicationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateNicknameCommandHandler(
        IUserCommunicationService userCommunicationService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userCommunicationService = userCommunicationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(UpdateNicknameCommand command)
    {
        // 取得 DeviceFingerprint
        var deviceFingerprint = _httpContextAccessor.HttpContext?.GetDeviceFingerprint();
        if (string.IsNullOrEmpty(deviceFingerprint))
            throw new UnauthorizedAccessException("無法驗證裝置指紋，請重新登入以繼續操作。");

        // 委派給 Service 處理業務邏輯
        await _userCommunicationService.UpdateNicknameAsync(command.NewNickname, deviceFingerprint);
    }
}
