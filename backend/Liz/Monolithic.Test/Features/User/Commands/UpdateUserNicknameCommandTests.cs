using FluentAssertions;
using Monolithic.Features.User.Commands;
using Monolithic.Shared.Common;

namespace Monolithic.Test.Features.User.Commands;

/// <summary>
/// UpdateUserNicknameCommand 的單元測試
/// </summary>
public class UpdateUserNicknameCommandTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var nickname = "TestUser";

        // Act
        var command = new UpdateUserNicknameCommand(userId, nickname);

        // Assert
        command.UserId.Should().Be(userId);
        command.Nickname.Should().Be(nickname);
    }

    [Theory]
    [InlineData("ValidNick")]
    [InlineData("用戶123")]
    [InlineData("A")]
    [InlineData("12345678901234567890123456789012")] // 32 字元
    public void IsValid_WithValidNickname_ShouldReturnTrue(string nickname)
    {
        // Arrange
        var command = new UpdateUserNicknameCommand(Guid.NewGuid(), nickname);

        // Act & Assert
        command.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123456789012345678901234567890123")] // 33 字元，超過限制
    public void IsValid_WithInvalidNickname_ShouldReturnFalse(string nickname)
    {
        // Arrange
        var command = new UpdateUserNicknameCommand(Guid.NewGuid(), nickname);

        // Act & Assert
        command.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetValidationError_WithValidNickname_ShouldReturnNull()
    {
        // Arrange
        var command = new UpdateUserNicknameCommand(Guid.NewGuid(), "ValidNick");

        // Act
        var error = command.GetValidationError();

        // Assert
        error.Should().BeNull();
    }

    [Fact]
    public void GetValidationError_WithInvalidNickname_ShouldReturnErrorCode()
    {
        // Arrange
        var command = new UpdateUserNicknameCommand(Guid.NewGuid(), "");

        // Act
        var error = command.GetValidationError();

        // Assert
        error.Should().Be(ErrorCodes.InvalidNickname);
    }
}
