using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.Communication;
using Monolithic.Features.User.Commands;
using Monolithic.Features.User.Queries;
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
            return Unauthorized(
                ApiResponse<object>.Fail(ErrorCode.AuthRequired, "UserId not found in cookies.")
            );
        }

        // 執行 UpdateNicknameCommand
        var result = await _mediator.Send(new UpdateNicknameCommand(userId, request.NewNickname));
        if (!result)
        {
            return BadRequest(ApiResponse<object>.Fail(ErrorCode.InvalidInput, "Failed to update nickname."));
        }

        // 通知本人所有連線（Clients.User 不適用匿名，改用 Clients.All + userId mapping）
        await _hubContext.Clients.All.SendAsync("NicknameUpdated", userId.ToString(), request.NewNickname);

        // 直接回傳資料物件，讓 ApiResponseResultFilter 自動包裝
        return Ok(new { newNickname = request.NewNickname });
    }

    /// <summary>
    /// 取得目前用戶資訊
    /// </summary>
    [HttpGet("me/profile")]
    public async Task<IActionResult> GetMyProfile()
    {
        // 取得 UserId（從 Cookie）
        if (
            !Request.Cookies.TryGetValue("UserId", out var userIdStr)
            || !Guid.TryParse(userIdStr, out var userId)
        )
        {
            return Unauthorized(
                ApiResponse<object>.Fail(ErrorCode.AuthRequired, "UserId not found in cookies.")
            );
        }

        // 查詢用戶資訊（目前僅有 Nickname，未來可擴充）
        var profile = await _mediator.Send(new GetProfileQuery(userId));
        if (string.IsNullOrWhiteSpace(profile?.Nickname))
        {
            return NotFound(ApiResponse<object>.Fail(ErrorCode.NotFound, "User not found."));
        }

        // 直接回傳資料物件，讓 ApiResponseResultFilter 自動包裝
        return Ok(new GetMyProfileResponse { Nickname = profile.Nickname });
    }
}

public class UpdateNicknameRequest
{
    public string NewNickname { get; set; } = string.Empty;
    // TODO: 若 UserId 需由前端傳遞，可加上 UserId 屬性
}

public class GetMyProfileResponse
{
    public string Nickname { get; set; } = string.Empty;
    // 未來可擴充更多欄位
}
