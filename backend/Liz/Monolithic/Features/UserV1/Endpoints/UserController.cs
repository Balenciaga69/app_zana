using MediatR;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Features.User.Commands;
using Monolithic.Features.User.Queries;
using Monolithic.Shared.Common;

namespace Monolithic.Features.User.Endpoints;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region User Management
    // TODO: @Copilot 盤點存在必要性
    [HttpGet("{id}")]
    public IActionResult GetUserById(string id)
    {
        // TODO: 實作根據用戶 ID 查詢用戶資訊的邏輯
        throw new NotImplementedException();
    }

    // TODO: @Copilot 盤點存在必要性
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<GetMeResult>>> GetMe()
    {
        var query = new GetMeQuery();
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<GetMeResult>.Ok(result, "取得用戶資訊成功"));
    }

    // TODO: @Copilot 盤點存在必要性
    [HttpGet("me/connections")]
    public IActionResult GetMyConnections()
    {
        // TODO: 實作取得當前用戶的連線資訊的邏輯
        throw new NotImplementedException();
    }

    #endregion

    #region Status Queries
    // TODO: @Copilot 盤點存在必要性
    [HttpGet("{id}/is-online")]
    public IActionResult IsUserOnline(string id)
    {
        // TODO: 實作檢查用戶是否在線的邏輯
        throw new NotImplementedException();
    }

    // TODO: @Copilot 盤點存在必要性
    [HttpGet("online-stats")]
    public IActionResult GetOnlineStats()
    {
        // TODO: 實作取得在線用戶統計資訊的邏輯
        throw new NotImplementedException();
    }

    // TODO: @Copilot 盤點存在必要性
    [HttpGet("{id}/rooms")]
    public IActionResult GetUserRooms(string id)
    {
        // TODO: 實作取得用戶所屬房間的邏輯
        throw new NotImplementedException();
    }
    #endregion
}
