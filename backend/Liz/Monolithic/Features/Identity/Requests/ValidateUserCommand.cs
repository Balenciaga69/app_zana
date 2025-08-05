using MediatR;
using Monolithic.Features.Identity.Models;

namespace Monolithic.Features.Identity.Requests;

/// <summary>
/// 驗證用戶身份的 Command
/// </summary>
public class ValidateUserCommand : IRequest<bool>
{
    public ValidateUserRequest Request { get; }

    public ValidateUserCommand(ValidateUserRequest request)
    {
        Request = request;
    }
}
