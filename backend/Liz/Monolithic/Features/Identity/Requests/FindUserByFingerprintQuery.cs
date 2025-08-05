using MediatR;
using Monolithic.Features.Identity.Models;

namespace Monolithic.Features.Identity.Requests;

/// <summary>
/// 依據指紋查找用戶的 Query
/// </summary>
public class FindUserByFingerprintQuery : IRequest<UserSession?>
{
    public FindUserByFingerprintRequest Request { get; }

    public FindUserByFingerprintQuery(FindUserByFingerprintRequest request)
    {
        Request = request;
    }
}
