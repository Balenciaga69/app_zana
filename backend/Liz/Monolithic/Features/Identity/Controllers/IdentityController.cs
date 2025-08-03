using Microsoft.AspNetCore.Mvc;
using Monolithic.Features.Identity.Models;
using Monolithic.Features.Identity.Services;
using Monolithic.Shared.Common;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IAppLogger<IdentityController> _appLogger;

    public IdentityController(IIdentityService identityService, IAppLogger<IdentityController> appLogger)
    {
        _identityService = identityService;
        _appLogger = appLogger;
    }

    /// <summary>
    /// 建立新用戶或找回現有用戶
    /// </summary>
    [HttpPost("create-or-retrieve")]
    public async Task<ActionResult<ApiResponse<UserSession>>> CreateOrRetrieveUser([FromBody] CreateUserRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;
        _appLogger.LogApiRequest("POST", "create-or-retrieve", request, traceId);

        try
        {
            // TODO: 加入請求驗證
            var userSession = await _identityService.CreateOrRetrieveUserAsync(request);
            return Ok(ApiResponse<UserSession>.Ok(userSession));
        }
        catch (Exception ex)
        {
            _appLogger.LogBusinessError(LogOperations.UserCreated, ex, request, traceId);
            return StatusCode(500, ApiResponse<UserSession>.Fail("建立或找回用戶失敗", ex.Message));
        }
    }

    /// <summary>
    /// 根據 UserId 取得用戶資訊
    /// </summary>
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<ApiResponse<UserSession>>> GetUser(Guid userId)
    {
        var traceId = HttpContext.TraceIdentifier;
        _appLogger.LogApiRequest("GET", $"{userId}", new { UserId = userId }, traceId);

        try
        {
            var userSession = await _identityService.GetUserByIdAsync(userId);
            if (userSession == null)
            {
                _appLogger.LogBusinessWarning(LogOperations.UserRetrieved, "用戶不存在", new { UserId = userId }, traceId);
                return NotFound(ApiResponse<UserSession>.Fail("用戶不存在", $"找不到 ID 為 {userId} 的用戶"));
            }

            return Ok(ApiResponse<UserSession>.Ok(userSession));
        }
        catch (Exception ex)
        {
            _appLogger.LogBusinessError(LogOperations.UserRetrieved, ex, new { UserId = userId }, traceId);
            return StatusCode(500, ApiResponse<UserSession>.Fail("取得用戶資訊失敗", ex.Message));
        }
    }

    /// <summary>
    /// 根據瀏覽器指紋查找用戶
    /// </summary>
    [HttpPost("find-by-fingerprint")]
    public async Task<ActionResult<ApiResponse<UserSession>>> FindByFingerprint([FromBody] FindUserByFingerprintRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;
        _appLogger.LogApiRequest("POST", "find-by-fingerprint", request, traceId);

        try
        {
            var userSession = await _identityService.FindUserByFingerprintAsync(request);
            if (userSession == null)
            {
                _appLogger.LogBusinessWarning(LogOperations.FingerprintMatched, "找不到符合指紋的用戶", request, traceId);
                return NotFound(ApiResponse<UserSession>.Fail("找不到用戶", "沒有找到符合指紋的用戶"));
            }

            return Ok(ApiResponse<UserSession>.Ok(userSession));
        }
        catch (Exception ex)
        {
            _appLogger.LogBusinessError(LogOperations.FingerprintMatched, ex, request, traceId);
            return StatusCode(500, ApiResponse<UserSession>.Fail("查找用戶失敗", ex.Message));
        }
    }

    /// <summary>
    /// 驗證用戶身份
    /// </summary>
    [HttpPost("validate")]
    public async Task<ActionResult<ApiResponse<bool>>> ValidateUser([FromBody] ValidateUserRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;
        _appLogger.LogApiRequest("POST", "validate", request, traceId);

        try
        {
            var isValid = await _identityService.ValidateUserAsync(request);
            return Ok(ApiResponse<bool>.Ok(isValid));
        }
        catch (Exception ex)
        {
            _appLogger.LogBusinessError(LogOperations.UserValidated, ex, request, traceId);
            return StatusCode(500, ApiResponse<bool>.Fail("驗證用戶身份失敗", ex.Message));
        }
    }

    /// <summary>
    /// 更新用戶活動時間
    /// </summary>
    [HttpPut("{userId:guid}/activity")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateActivity(Guid userId, [FromBody] CreateUserRequest? deviceInfo = null)
    {
        var traceId = HttpContext.TraceIdentifier;
        _appLogger.LogApiRequest("PUT", $"{userId}/activity", new { UserId = userId, DeviceInfo = deviceInfo }, traceId);
        try
        {
            await _identityService.UpdateUserActivityAsync(userId, deviceInfo);
            return Ok(ApiResponse<object>.Ok(new { }, "用戶活動時間已更新"));
        }
        catch (Exception ex)
        {
            _appLogger.LogBusinessError(LogOperations.UserActivityUpdated, ex, new { UserId = userId, DeviceInfo = deviceInfo }, traceId);
            return StatusCode(500, ApiResponse<object>.Fail("更新用戶活動時間失敗", ex.Message));
        }
    }

    /// <summary>
    /// 設定用戶上線狀態
    /// </summary>
    [HttpPut("{userId:guid}/online-status")]
    public async Task<ActionResult<ApiResponse<object>>> SetOnlineStatus(Guid userId, [FromBody] bool isOnline)
    {
        var traceId = HttpContext.TraceIdentifier;
        _appLogger.LogApiRequest("PUT", $"{userId}/online-status", new { UserId = userId, IsOnline = isOnline }, traceId);

        try
        {
            await _identityService.SetUserOnlineStatusAsync(userId, isOnline);
            return Ok(ApiResponse<object>.Ok(new { }, $"用戶上線狀態已設為 {(isOnline ? "上線" : "離線")}"));
        }
        catch (Exception ex)
        {
            _appLogger.LogBusinessError(LogOperations.UserStatusChanged, ex, new { UserId = userId, IsOnline = isOnline }, traceId);
            return StatusCode(500, ApiResponse<object>.Fail("設定用戶上線狀態失敗", ex.Message));
        }
    }
}
