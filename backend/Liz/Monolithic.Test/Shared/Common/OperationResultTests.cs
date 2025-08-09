using FluentAssertions;
using Monolithic.Shared.Common;

namespace Monolithic.Test.Shared.Common;

/// <summary>
/// OperationResult 類別的單元測試
/// </summary>
public class OperationResultTests
{
    [Fact]
    public void Ok_WithData_ShouldReturnSuccessResult()
    {
        // Arrange
        var testData = "Test Data";

        // Act
        var result = OperationResult<string>.Ok(testData);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().Be(testData);
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Fail_WithErrorCode_ShouldReturnFailResult()
    {
        // Arrange
        var errorCode = ErrorCodes.UserNotFound;

        // Act
        var result = OperationResult<string>.Fail(errorCode);

        // Assert
        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(ErrorMessages.UserNotFound);
    }

    [Fact]
    public void Fail_WithCustomMessage_ShouldReturnFailResult()
    {
        // Arrange
        var errorCode = ErrorCodes.InvalidInput;
        var customMessage = "Custom error message";

        // Act
        var result = OperationResult<string>.Fail(errorCode, customMessage);

        // Assert
        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(customMessage);
    }

    [Fact]
    public void ToApiResponse_SuccessResult_ShouldReturnSuccessApiResponse()
    {
        // Arrange
        var testData = "Test Data";
        var result = OperationResult<string>.Ok(testData);

        // Act
        var apiResponse = result.ToApiResponse();

        // Assert
        apiResponse.Success.Should().BeTrue();
        apiResponse.Data.Should().Be(testData);
        apiResponse.Code.Should().Be("OK");
        apiResponse.Message.Should().Be("操作成功");
    }

    [Fact]
    public void ToApiResponse_FailResult_ShouldReturnFailApiResponse()
    {
        // Arrange
        var errorCode = ErrorCodes.UserNotFound;
        var result = OperationResult<string>.Fail(errorCode);

        // Act
        var apiResponse = result.ToApiResponse();

        // Assert
        apiResponse.Success.Should().BeFalse();
        apiResponse.Data.Should().BeNull();
        apiResponse.Code.Should().Be(errorCode);
        apiResponse.Message.Should().Be(ErrorMessages.UserNotFound);
    }

    [Fact]
    public void OperationResult_WithoutGeneric_ShouldWork()
    {
        // Act
        var successResult = OperationResult.Ok();
        var failResult = OperationResult.Fail(ErrorCodes.InternalServerError);

        // Assert
        successResult.Success.Should().BeTrue();
        failResult.Success.Should().BeFalse();
        failResult.ErrorCode.Should().Be(ErrorCodes.InternalServerError);
        failResult.ErrorMessage.Should().Be(ErrorMessages.InternalServerError);
    }
}
