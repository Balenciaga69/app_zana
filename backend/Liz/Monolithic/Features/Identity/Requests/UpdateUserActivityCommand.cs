using MediatR;
using Monolithic.Features.Identity.Models;

namespace Monolithic.Features.Identity.Requests;

/// <summary>
/// 更新用戶活動時間的 Command
/// </summary>
public class UpdateUserActivityCommand : IRequest<Unit>
{
    public Guid UserId { get; }
    public CreateUserRequest? DeviceInfo { get; }

    public UpdateUserActivityCommand(Guid userId, CreateUserRequest? deviceInfo = null)
    {
        UserId = userId;
        DeviceInfo = deviceInfo;
    }
}
