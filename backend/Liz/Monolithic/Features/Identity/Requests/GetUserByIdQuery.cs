using MediatR;
using Monolithic.Features.Identity.Models;

namespace Monolithic.Features.Identity.Requests;

/// <summary>
/// 根據 UserId 取得用戶資訊的 Query
/// </summary>
public class GetUserByIdQuery : IRequest<UserSession?>
{
    public Guid UserId { get; set; }

    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}
