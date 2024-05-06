using AutoMapper;
using FastFoodUserManagement.Application.UseCases.CreateUser;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Entities;
using FastFoodUserManagement.Domain.Validations;
using Moq;

namespace FastFoodUserManagement.Tests.Core.Application.UseCases.CreateUser;

public class CreateUserHandlerTests
{
    [Fact]
    public async Task Handle_UserCreation_Success()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var userCreationMock = new Mock<IUserCreation>();
        var mapperMock = new Mock<IMapper>();
        var validationNotificationsMock = new Mock<IValidationNotifications>();

        var handler = new CreateUserHandler(userRepositoryMock.Object, mapperMock.Object, validationNotificationsMock.Object, userCreationMock.Object);
        var cancellationToken = new CancellationToken();
        var request = new CreateUserRequest("John Doe", "john@example.com", "123.456.789-00");

        var userEntity = new UserEntity { Email = "john@example.com", Identification = "12345678900" };
        var cognitoUserIdentification = "testCognitoId";

        mapperMock.Setup(m => m.Map<UserEntity>(request)).Returns(userEntity);
        userRepositoryMock.Setup(u => u.GetUserByCPFOrEmailAsync(userEntity.Identification, userEntity.Email, cancellationToken))
            .ReturnsAsync((UserEntity)null);

        userCreationMock.Setup(u => u.CreateUser(userEntity, cancellationToken))
            .ReturnsAsync(cognitoUserIdentification);

        // Act
        var response = await handler.Handle(request, cancellationToken);

        // Assert
        userRepositoryMock.Verify(u => u.AddUserAsync(userEntity, cancellationToken), Times.Once);
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Handle_UserAlreadyExists_ValidationFailure()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var userCreationMock = new Mock<IUserCreation>();
        var mapperMock = new Mock<IMapper>();
        var validationNotificationsMock = new Mock<IValidationNotifications>();

        var handler = new CreateUserHandler(userRepositoryMock.Object, mapperMock.Object, validationNotificationsMock.Object, userCreationMock.Object);
        var cancellationToken = new CancellationToken();
        var request = new CreateUserRequest("John Doe", "john@example.com", "123.456.789-00");

        var userEntity = new UserEntity { Email = "john@example.com", Identification = "12345678900" };

        mapperMock.Setup(m => m.Map<UserEntity>(request)).Returns(userEntity);
        userRepositoryMock.Setup(u => u.GetUserByCPFOrEmailAsync(userEntity.Identification, userEntity.Email, cancellationToken))
            .ReturnsAsync(userEntity);

        // Act
        var response = await handler.Handle(request, cancellationToken);

        // Assert
        userRepositoryMock.Verify(u => u.AddUserAsync(It.IsAny<UserEntity>(), cancellationToken), Times.Never);
        validationNotificationsMock.Verify(v => v.AddError("Identification", "Já existe um usuário cadastrado com esse CPF ou e-mail."), Times.Once);
        Assert.NotNull(response);
    }
}
