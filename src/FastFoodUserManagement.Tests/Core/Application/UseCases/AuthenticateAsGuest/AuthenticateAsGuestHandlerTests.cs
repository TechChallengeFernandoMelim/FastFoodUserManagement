using FastFoodUserManagement.Application.UseCases.AuthenticateAsGuest;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Entities;
using Moq;

namespace FastFoodUserManagement.Tests.Core.Application.UseCases.AuthenticateAsGuest;

public class AuthenticateAsGuestHandlerTests
{
    [Fact]
    public async Task Handle_GuestAuthentication_Success()
    {
        // Arrange
        var userAuthenticationMock = new Mock<IUserAuthentication>();
        var handler = new AuthenticateAsGuestHandler(userAuthenticationMock.Object);
        var cancellationToken = new CancellationToken();
        var expectedToken = "guestToken";
        var validator = new AuthenticateAsGuestValidator();

        Environment.SetEnvironmentVariable("GUEST_EMAIL", "guest@example.com");
        Environment.SetEnvironmentVariable("GUEST_IDENTIFICATION", "guestPassword");

        userAuthenticationMock.Setup(u => u.AuthenticateUser(It.IsAny<UserEntity>(), cancellationToken))
            .ReturnsAsync(expectedToken);

        var request = new AuthenticateAsGuestRequest();

        // Act
        var response = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.Equal(expectedToken, response.Token);
    }
}
