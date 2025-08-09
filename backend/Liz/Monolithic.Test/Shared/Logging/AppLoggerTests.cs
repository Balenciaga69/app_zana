using Microsoft.Extensions.Logging;
using Monolithic.Shared.Logging;
using Moq;

namespace Monolithic.Test.Shared.Logging
{
    public class AppLoggerTests
    {
        private readonly Mock<ILogger<TestService>> _mockLogger;
        private readonly AppLogger<TestService> _appLogger;

        public AppLoggerTests()
        {
            _mockLogger = new Mock<ILogger<TestService>>();
            _appLogger = new AppLogger<TestService>(_mockLogger.Object);
        }

        [Fact]
        public void LogInfo_ShouldLogInformationWithCorrectFormat()
        {
            // Arrange
            var message = "Test info message";
            var data = new { Key = "Value" };

            // Act
            _appLogger.LogInfo(message, data);

            // Assert
            VerifyLog(LogLevel.Information, "[Info] TestService | Test info message", null);
        }

        [Fact]
        public void LogWarn_ShouldLogWarningWithCorrectFormat()
        {
            // Arrange
            var message = "Test warning message";
            var data = new { Key = "Value" };

            // Act
            _appLogger.LogWarn(message, data);

            // Assert
            VerifyLog(LogLevel.Warning, "[Warn] TestService | Test warning message", null);
        }

        [Fact]
        public void LogError_ShouldLogErrorWithCorrectFormat()
        {
            // Arrange
            var message = "Test error message";
            var exception = new Exception("Test exception");
            var data = new { Key = "Value" };

            // Act
            _appLogger.LogError(message, exception, data);

            // Assert
            VerifyLog(LogLevel.Error, "[Error] TestService | Test error message", exception);
        }

        private void VerifyLog(LogLevel logLevel, string expectedMessage, Exception? exception)
        {
            _mockLogger.Verify(
                x =>
                    x.Log(
                        logLevel,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => LogMessageMatches(v, expectedMessage)),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                    ),
                Times.Once
            );
        }

        private bool LogMessageMatches(object? value, string expectedMessage)
        {
            return value?.ToString()?.Contains(expectedMessage) == true;
        }

        public class TestService { }
    }
}
