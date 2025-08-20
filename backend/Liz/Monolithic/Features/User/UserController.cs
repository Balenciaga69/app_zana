using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.Communication;
using Monolithic.Shared.Common;

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
        // 取得 UserId（從 Cookie）
        if (
            !Request.Cookies.TryGetValue("UserId", out var userIdStr)
            || !Guid.TryParse(userIdStr, out var userId)
        )
        {
            // 統一 API Response 格式
            return Unauthorized(
                ApiResponse<object>.Fail(ErrorCode.AuthRequired, "UserId not found in cookies.")
            );
        }

        // 執行 UpdateNicknameCommand
        var result = await _mediator.Send(new Commands.UpdateNicknameCommand(userId, request.NewNickname));
        if (!result)
        {
            // 統一 API Response 格式
            return BadRequest(ApiResponse<object>.Fail(ErrorCode.InvalidInput, "Failed to update nickname."));
        }

        // 通知本人所有連線（Clients.User 不適用匿名，改用 Clients.All + userId mapping）
        // TODO: @Copilot 這裡業務邏輯未來會變更 可能要思考
        await _hubContext.Clients.All.SendAsync("NicknameUpdated", userId.ToString(), request.NewNickname);

        // 回傳標準 API Response
        return Ok(
            ApiResponse<object>.Ok(
                new { newNickname = request.NewNickname },
                "Nickname updated successfully."
            )
        );
    }
}

public class UpdateNicknameRequest
{
    public string NewNickname { get; set; } = string.Empty;
    // TODO: 若 UserId 需由前端傳遞，可加上 UserId 屬性
}
