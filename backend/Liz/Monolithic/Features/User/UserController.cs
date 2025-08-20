using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.Communication;

namespace Monolithic.Features.User;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<CommunicationHub> _hubContext;

    public UserController(IMediator mediator, IHubContext<CommunicationHub> hubContext)
    {
        _mediator = mediator;
        _hubContext = hubContext;
    }

    /// <summary>
    /// 更新使用者暱稱
    /// </summary>
    [HttpPut("nickname")]
    public async Task<IActionResult> UpdateNickname([FromBody] UpdateNicknameRequest request)
    {
        // TODO: 1. 取得 UserId（可從 Claims, Header, Cookie, 或前端傳遞）
        // TODO: 2. 執行 UpdateNicknameCommand
        // TODO: 3. 若成功，通知 CommunicationHub：
        //   - 通知本人（Clients.User or Clients.Client）
        //   - 通知同房間活躍用戶（Clients.Group or 自訂邏輯）
        // TODO: 4. 回傳標準 API Response
        throw new NotImplementedException();
    }
}

public class UpdateNicknameRequest
{
    public string NewNickname { get; set; } = string.Empty;
    // TODO: 若 UserId 需由前端傳遞，可加上 UserId 屬性
}
