using AutoMapper;
using FastFoodUserManagement.Application.UseCases.AuthenticateUser;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Entities;
using FastFoodUserManagement.Domain.Exceptions;
using Moq;

namespace FastFoodUserManagement.Tests.Core.Application.UseCases.AuthenticateUser
{
    public class AuthenticateUserHandlerTests
    {
        [Fact]
        public async Task Handle_UserAuthentication_Success()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userAuthenticationMock = new Mock<IUserAuthentication>();
            var mapperMock = new Mock<IMapper>();

            var handler = new AuthenticateUserHandler(userRepositoryMock.Object, mapperMock.Object, userAuthenticationMock.Object);
            var cancellationToken = new CancellationToken();
            var request = new AuthenticateUserRequest("12345678900");

            var userEntity = new UserEntity { Email = "test@example.com", Identification = "password123" };
            var expectedToken = "testToken";

            userRepositoryMock.Setup(u => u.GetUserByCPFOrEmailAsync(request.cpf, string.Empty, cancellationToken))
                .ReturnsAsync(userEntity);

            userAuthenticationMock.Setup(u => u.AuthenticateUser(userEntity, cancellationToken))
                .ReturnsAsync(expectedToken);

            var expectedResponse = new AuthenticateUserResponse { Email = userEntity.Email, Token = expectedToken };
            mapperMock.Setup(m => m.Map<AuthenticateUserResponse>(userEntity)).Returns(expectedResponse);

            // Act
            var response = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal(expectedResponse.Email, response.Email);
            Assert.Equal(expectedResponse.Token, response.Token);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsObjectNotFoundException()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userAuthenticationMock = new Mock<IUserAuthentication>();
            var mapperMock = new Mock<IMapper>();

            var handler = new AuthenticateUserHandler(userRepositoryMock.Object, mapperMock.Object, userAuthenticationMock.Object);
            var cancellationToken = new CancellationToken();
            var request = new AuthenticateUserRequest("12345678900");

            userRepositoryMock.Setup(u => u.GetUserByCPFOrEmailAsync(request.cpf, string.Empty, cancellationToken))
                .ReturnsAsync((UserEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => handler.Handle(request, cancellationToken));
        }

        [Fact]
        public async Task Handle_AuthenticationFails_ThrowsException()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userAuthenticationMock = new Mock<IUserAuthentication>();
            var mapperMock = new Mock<IMapper>();

            var handler = new AuthenticateUserHandler(userRepositoryMock.Object, mapperMock.Object, userAuthenticationMock.Object);
            var cancellationToken = new CancellationToken();
            var request = new AuthenticateUserRequest("12345678900");

            var userEntity = new UserEntity { Email = "test@example.com", Identification = "password123" };
            userRepositoryMock.Setup(u => u.GetUserByCPFOrEmailAsync(request.cpf, string.Empty, cancellationToken))
                .ReturnsAsync(userEntity);

            userAuthenticationMock.Setup(u => u.AuthenticateUser(userEntity, cancellationToken))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, cancellationToken));
        }
    }
}
