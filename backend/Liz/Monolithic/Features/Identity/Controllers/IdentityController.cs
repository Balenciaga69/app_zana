using MediatR;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Features.Identity.Models;
using Monolithic.Features.Identity.Requests;
using Monolithic.Shared.Common;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAppLogger<IdentityController> _appLogger;

    public IdentityController(IMediator mediator, IAppLogger<IdentityController> appLogger)
    {
        _mediator = mediator;
        _appLogger = appLogger;
    }

    /// <summary>
    /// 建立新用戶或找回現有用戶 (MediatR)
    /// </summary>
    [HttpPost("create-or-retrieve")]
    public async Task<ActionResult<ApiResponse<UserSession>>> CreateOrRetrieveUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var command = new CreateOrRetrieveUserCommand
            {
                BrowserFingerprint = request.BrowserFingerprint,
                UserAgent = request.UserAgent,
                IpAddress = request.IpAddress,
                DeviceType = request.DeviceType,
                OperatingSystem = request.OperatingSystem,
                Browser = request.Browser,
                BrowserVersion = request.BrowserVersion,
                Platform = request.Platform,
            };
            var userSession = await _mediator.Send(command);
            return Ok(ApiResponse<UserSession>.Ok(userSession));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UserSession>.Fail("建立或找回用戶失敗", ex.Message));
        }
    }

    /// <summary>
    /// 根據 UserId 取得用戶資訊 (MediatR)
    /// </summary>
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<ApiResponse<UserSession>>> GetUser(Guid userId)
    {
        try
        {
            var query = new GetUserByIdQuery(userId);
            var userSession = await _mediator.Send(query);
            if (userSession == null)
            {
                _appLogger.LogWarn("用戶不存在", new { UserId = userId });
                return NotFound(ApiResponse<UserSession>.Fail("用戶不存在", $"找不到 ID 為 {userId} 的用戶"));
            }
            return Ok(ApiResponse<UserSession>.Ok(userSession));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UserSession>.Fail("取得用戶資訊失敗", ex.Message));
        }
    }

    /// <summary>
    /// 根據瀏覽器指紋查找用戶 (MediatR)
    /// </summary>
    [HttpPost("find-by-fingerprint")]
    public async Task<ActionResult<ApiResponse<UserSession>>> FindByFingerprint([FromBody] FindUserByFingerprintRequest request)
    {
        try
        {
            var query = new FindUserByFingerprintQuery(request);
            var userSession = await _mediator.Send(query);
            if (userSession == null)
            {
                _appLogger.LogWarn("找不到符合指紋的用戶", request);
                return NotFound(ApiResponse<UserSession>.Fail("找不到用戶", "沒有找到符合指紋的用戶"));
            }
            return Ok(ApiResponse<UserSession>.Ok(userSession));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UserSession>.Fail("查找用戶失敗", ex.Message));
        }
    }

    /// <summary>
    /// 驗證用戶身份 (MediatR)
    /// </summary>
    [HttpPost("validate")]
    public async Task<ActionResult<ApiResponse<bool>>> ValidateUser([FromBody] ValidateUserRequest request)
    {
        try
        {
            var command = new ValidateUserCommand(request);
            var isValid = await _mediator.Send(command);
            return Ok(ApiResponse<bool>.Ok(isValid));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.Fail("驗證用戶身份失敗", ex.Message));
        }
    }

    /// <summary>
    /// 更新用戶活動時間 (MediatR)
    /// </summary>
    [HttpPut("{userId:guid}/activity")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateActivity(Guid userId, [FromBody] CreateUserRequest? deviceInfo = null)
    {
        try
        {
            var command = new UpdateUserActivityCommand(userId, deviceInfo);
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.Ok(new { }, "用戶活動時間已更新"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.Fail("更新用戶活動時間失敗", ex.Message));
        }
    }

    /// <summary>
    /// 設定用戶上線狀態 (MediatR)
    /// </summary>
    [HttpPut("{userId:guid}/online-status")]
    public async Task<ActionResult<ApiResponse<object>>> SetOnlineStatus(Guid userId, [FromBody] bool isOnline)
    {
        try
        {
            var command = new SetUserOnlineStatusCommand(userId, isOnline);
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.Ok(new { }, $"用戶上線狀態已設為 {(isOnline ? "上線" : "離線")}"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.Fail("設定用戶上線狀態失敗", ex.Message));
        }
    }
}
