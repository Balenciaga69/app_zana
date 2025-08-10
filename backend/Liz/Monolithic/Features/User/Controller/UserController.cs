using MediatR;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Features.User.Commands;
using Monolithic.Features.User.Queries;
using Monolithic.Shared.Common;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAppLogger<UserController> _logger;

    public UserController(IMediator mediator, IAppLogger<UserController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 註冊用戶或重新連線
    /// POST /api/users/register
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<RegisterUserResult>>> RegisterUser(
        [FromBody] RegisterUserRequest request
    )
    {
        try
        {
            var command = new RegisterUserCommand
            {
                ExistingUserId = request.ExistingUserId,
                DeviceFingerprint = request.DeviceFingerprint,
                UserAgent = request.UserAgent,
                IpAddress = request.IpAddress,
            };

            var operationResult = await _mediator.Send(command);

            if (operationResult.Success)
            {
                _logger.LogInfo(
                    "用戶註冊成功",
                    new { operationResult.Data!.UserId, operationResult.Data.IsNewUser }
                );

                var message = operationResult.Data.IsNewUser ? "新用戶註冊成功" : "用戶重新連線成功";
                return Ok(ApiResponse<RegisterUserResult>.Ok(operationResult.Data, message));
            }
            else
            {
                _logger.LogWarn(
                    "用戶註冊失敗",
                    new
                    {
                        request.DeviceFingerprint,
                        operationResult.ErrorCode,
                        operationResult.ErrorMessage,
                    }
                );
                return BadRequest(
                    ApiResponse<RegisterUserResult>.Fail(
                        operationResult.ErrorCode!.Value,
                        operationResult.ErrorMessage!
                    )
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("用戶註冊異常", ex, new { request.DeviceFingerprint });
            return StatusCode(500, ApiResponse<RegisterUserResult>.Fail(ErrorCode.InternalServerError));
        }
    }

    /// <summary>
    /// 取得當前用戶資訊
    /// GET /api/users/me
    /// </summary>
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<GetUserByIdResult>>> GetCurrentUser([FromQuery] Guid userId)
    {
        try
        {
            var query = new GetUserByIdQuery(userId);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound(ApiResponse<GetUserByIdResult>.Fail(ErrorCode.UserNotFound));
            }

            return Ok(ApiResponse<GetUserByIdResult>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError("取得用戶資訊失敗", ex, new { userId });
            return StatusCode(500, ApiResponse<GetUserByIdResult>.Fail(ErrorCode.InternalServerError));
        }
    }

    /// <summary>
    /// 取得指定用戶資訊
    /// GET /api/users/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<GetUserByIdResult>>> GetUser(Guid id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound(
                    ApiResponse<GetUserByIdResult>.Fail(ErrorCode.UserNotFound, $"找不到 ID 為 {id} 的用戶")
                );
            }

            return Ok(ApiResponse<GetUserByIdResult>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError("取得用戶資訊失敗", ex, new { id });
            return StatusCode(500, ApiResponse<GetUserByIdResult>.Fail(ErrorCode.InternalServerError));
        }
    }

    /// <summary>
    /// 更新當前用戶暱稱
    /// PUT /api/users/me/nickname
    /// </summary>
    [HttpPut("me/nickname")]
    public async Task<ActionResult<ApiResponse<UpdateUserNicknameResult>>> UpdateNickname(
        [FromBody] UpdateNicknameRequest request
    )
    {
        try
        {
            var command = new UpdateUserNicknameCommand(request.UserId, request.Nickname);
            var operationResult = await _mediator.Send(command);

            // 直接轉換 OperationResult 為 ApiResponse
            var apiResponse = operationResult.ToApiResponse();

            if (operationResult.Success)
            {
                _logger.LogInfo("暱稱更新成功", new { request.UserId, request.Nickname });
                return Ok(apiResponse);
            }
            else
            {
                _logger.LogWarn(
                    "暱稱更新失敗",
                    new
                    {
                        request.UserId,
                        request.Nickname,
                        operationResult.ErrorCode,
                    }
                );
                return BadRequest(apiResponse);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("更新暱稱失敗", ex, new { request.UserId, request.Nickname });
            return StatusCode(500, ApiResponse<UpdateUserNicknameResult>.Fail(ErrorCode.InternalServerError));
        }
    }

    /// <summary>
    /// 取得用戶連線歷史
    /// GET /api/users/me/connections
    /// </summary>
    [HttpGet("me/connections")]
    public async Task<ActionResult<ApiResponse<GetUserConnectionsResult>>> GetConnections(
        [FromQuery] Guid userId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20
    )
    {
        try
        {
            var query = new GetUserConnectionsQuery(userId, skip, take);
            var result = await _mediator.Send(query);

            return Ok(ApiResponse<GetUserConnectionsResult>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "取得連線歷史失敗",
                ex,
                new
                {
                    userId,
                    skip,
                    take,
                }
            );
            return StatusCode(500, ApiResponse<GetUserConnectionsResult>.Fail(ErrorCode.InternalServerError));
        }
    }

    /// <summary>
    /// 驗證設備指紋
    /// POST /api/users/device-verify
    /// </summary>
    [HttpPost("device-verify")]
    public async Task<ActionResult<ApiResponse<GetUserByDeviceFingerprintResult>>> VerifyDeviceFingerprint(
        [FromBody] DeviceFingerprintRequest request
    )
    {
        try
        {
            var query = new GetUserByDeviceFingerprintQuery(request.DeviceFingerprint);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound(
                    ApiResponse<GetUserByDeviceFingerprintResult>.Fail(
                        ErrorCode.UserNotFound,
                        "找不到對應的用戶"
                    )
                );
            }

            return Ok(ApiResponse<GetUserByDeviceFingerprintResult>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError("驗證設備指紋失敗", ex, new { request.DeviceFingerprint });
            return StatusCode(
                500,
                ApiResponse<GetUserByDeviceFingerprintResult>.Fail(ErrorCode.InternalServerError)
            );
        }
    }
}

/// <summary>
/// 用戶註冊請求
/// </summary>
public class RegisterUserRequest
{
    public Guid? ExistingUserId { get; set; }
    public string DeviceFingerprint { get; set; } = default!;
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
}

/// <summary>
/// 更新暱稱請求
/// </summary>
public class UpdateNicknameRequest
{
    public Guid UserId { get; set; }
    public string Nickname { get; set; } = default!;
}

/// <summary>
/// 設備指紋請求
/// </summary>
public class DeviceFingerprintRequest
{
    public string DeviceFingerprint { get; set; } = default!;
}
