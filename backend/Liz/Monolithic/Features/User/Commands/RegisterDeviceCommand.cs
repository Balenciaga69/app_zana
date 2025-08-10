using MediatR;
using Monolithic.Features.User.Repositories;

namespace Monolithic.Features.User.Commands;

public class RegisterDeviceCommand : IRequest<bool>
{
    public string DeviceFingerprint { get; set; }
    public Guid? ExistingUserId { get; set; }

    public RegisterDeviceCommand(string deviceFingerprint, Guid? existingUserId = null)
    {
        DeviceFingerprint = deviceFingerprint;
        ExistingUserId = existingUserId;
    }
}

public class RegisterDeviceCommandHandler : IRequestHandler<RegisterDeviceCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public RegisterDeviceCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(RegisterDeviceCommand request, CancellationToken cancellationToken)
    {
        // 檢查設備指紋是否已經存在於資料庫中
        var fingerprintExists = await _userRepository.CheckDeviceFingerprintExistsAsync(
            request.DeviceFingerprint
        );

        if (fingerprintExists)
            throw new InvalidOperationException($"設備指紋 '{request.DeviceFingerprint}' 已經被註冊。");

        if (request.ExistingUserId.HasValue)
        {
            // 如果提供了 ExistingUserId，更新該用戶的設備指紋
            await _userRepository.UpdateUserDeviceFingerprintAsync(
                request.ExistingUserId.Value,
                request.DeviceFingerprint
            );
        }
        else
        {
            // 如果未提供 ExistingUserId，創建一個新用戶並綁定設備指紋
            await _userRepository.CreateNewUserAsync(request.DeviceFingerprint);
        }

        // 返回操作成功的結果
        return true;
    }
}
