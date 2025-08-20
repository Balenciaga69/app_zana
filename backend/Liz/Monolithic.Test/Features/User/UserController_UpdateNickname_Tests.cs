using FluentAssertions;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.Communication;
using Monolithic.Features.User;
using Monolithic.Shared.Common;
using Moq;
using Xunit;

// ...existing code...

namespace Monolithic.Test.Features.User;

public class UserController_UpdateNickname_Tests
{
    /*
     驗證 UpdateNickname
     在請求有效時
     應該回傳 Ok 並正確更新暱稱
     */
    [Fact]
    public async Task UpdateNickname_Should_Return_Ok_And_Update_When_Valid_Request()
    {
        // Arrange
        var mediator = new Moq.Mock<MediatR.IMediator>();
        var hubContext = new Moq.Mock<IHubContext<CommunicationHub>>();
        var request = new UpdateNicknameRequest { NewNickname = "新暱稱" };
        var userId = Guid.NewGuid();
        // 模擬 MediatR 成功
        mediator.Setup(m => m.Send(It.IsAny<object>(), default)).ReturnsAsync(true);
        var controller = new UserController(mediator.Object, hubContext.Object);
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
        // 正確設置 Cookie（用 Header 方式，RequestCookieCollection 不能直接 new）
        var cookieValue = $"UserId={userId}";
        controller.ControllerContext.HttpContext.Request.Headers["Cookie"] = cookieValue;

        // Act
        var result = await controller.UpdateNickname(request);

        // Assert
        result.Should().BeOfType<ApiResponse<object>>();
        var ok = result as ApiResponse<object>;
        ok.Should().NotBeNull();
        // 可進一步驗證回傳內容與訊息(TODO:目前該測試有錯誤)
    }

    /*
     驗證 UpdateNickname
     當暱稱為空時
     應該回傳 BadRequest
     */
    [Fact]
    public async Task UpdateNickname_Should_Return_BadRequest_When_Nickname_Is_Empty()
    {
        // Arrange
        // ...
        // Act
        // ...
        // Assert
        // ...
    }

    /*
     驗證 UpdateNickname
     當暱稱已存在時
     應該回傳 Conflict
     */
    [Fact]
    public async Task UpdateNickname_Should_Return_Conflict_When_Nickname_Already_Exists()
    {
        // Arrange
        // ...
        // Act
        // ...
        // Assert
        // ...
    }

    /*
     驗證 UpdateNickname
     當用戶不存在時
     應該回傳 NotFound
     */
    [Fact]
    public async Task UpdateNickname_Should_Return_NotFound_When_User_Not_Exist()
    {
        // Arrange
        // ...
        // Act
        // ...
        // Assert
        // ...
    }

    /*
     驗證 UpdateNickname
     當非本人嘗試修改暱稱時
     應該回傳 Forbidden
     */
    [Fact]
    public async Task UpdateNickname_Should_Return_Forbidden_When_Not_Owner()
    {
        // Arrange
        // ...
        // Act
        // ...
        // Assert
        // ...
    }
}
