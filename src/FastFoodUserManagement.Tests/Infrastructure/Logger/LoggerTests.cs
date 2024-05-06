using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Moq;

namespace FastFoodUserManagement.Tests.Infrastructure.Logger;

public class LoggerTests
{
    Mock<BasicAWSCredentials> _credentialsMock;

    public LoggerTests()
    {
        _credentialsMock = new Mock<BasicAWSCredentials>("accesskey", "secretekeys");
    }

    [Fact]
    public async Task Log_ValidInput_Success()
    {
        // Arrange
        var stackTrace = "Test stack trace";
        var message = "Test message";
        var exception = "Test exception";

        var sqsClientMock = new Mock<AmazonSQSClient>(_credentialsMock.Object, RegionEndpoint.USEast1);
        sqsClientMock.Setup(s => s.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new SendMessageResponse());

        var sqsService = new FastFoodUserManagement.Infrastructure.SQS.Logger.Logger(sqsClientMock.Object);

        // Act
        await sqsService.Log(stackTrace, message, exception);

        // Assert
        sqsClientMock.Verify(s => s.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
