using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using FastFoodUserManagement.Domain.Entities;
using FastFoodUserManagement.Infrastructure.Cognito.Creation;
using Moq;

namespace FastFoodUserManagement.Tests.Infrastructure.Cognito
{
    public class CognitoUserCreationTests
    {
        [Fact]
        public async Task CreateUser_ValidUser_ReturnsUsername()
        {
            // Arrange
            var cognitoMock = new Mock<AmazonCognitoIdentityProviderClient>();
            var user = new UserEntity { Email = "test@example.com", Identification = "password123" };
            var cancellationToken = new CancellationToken();
            var expectedUsername = "test@example.com";
            var expectedUserPoolId = "testUserPoolId";

            var userCreation = new CognitoUserCreation(cognitoMock.Object);

            Environment.SetEnvironmentVariable("AWS_USER_POOL_ID", expectedUserPoolId);

            cognitoMock.Setup(x => x.AdminCreateUserAsync(It.IsAny<AdminCreateUserRequest>(), cancellationToken))
                       .ReturnsAsync(new AdminCreateUserResponse { User = new UserType { Username = expectedUsername } });

            cognitoMock.Setup(x => x.AdminSetUserPasswordAsync(It.IsAny<AdminSetUserPasswordRequest>(), cancellationToken))
                       .ReturnsAsync(new AdminSetUserPasswordResponse());

            // Act
            var result = await userCreation.CreateUser(user, cancellationToken);

            // Assert
            Assert.Equal(expectedUsername, result);
        }
    }
}
