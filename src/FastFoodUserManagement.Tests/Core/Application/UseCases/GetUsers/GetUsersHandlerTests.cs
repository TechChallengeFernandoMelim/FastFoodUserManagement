using AutoMapper;
using FastFoodUserManagement.Application.UseCases.GetUsers;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Entities;
using Moq;

namespace FastFoodUserManagement.Tests.Core.Application.UseCases.GetUsers;

public class GetUsersHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsExpectedUsers()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        var handler = new GetUsersHandler(userRepositoryMock.Object, mapperMock.Object);
        var cancellationToken = new CancellationToken();

        var users = new List<UserEntity>
            {
                new UserEntity { Email = "user1@example.com", Identification = "123456789" },
                new UserEntity { Email = "user2@example.com", Identification = "987654321" }
            };

        var expectedUsers = new List<User>
            {
                new User { Email = "user1@example.com", Identification = "123456789" },
                new User { Email = "user2@example.com", Identification = "987654321" }
            };

        userRepositoryMock.Setup(repo => repo.GetUsersAsync(cancellationToken)).ReturnsAsync(users);
        mapperMock.Setup(mapper => mapper.Map<IEnumerable<User>>(users)).Returns(expectedUsers);

        // Act
        var response = await handler.Handle(new GetUsersRequest(), cancellationToken);

        // Assert
        Assert.Equal(expectedUsers, response.Users);
    }

    [Fact]
    public async Task Handle_EmptyUsers_ReturnsEmptyList()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();

        var handler = new GetUsersHandler(userRepositoryMock.Object, mapperMock.Object);
        var cancellationToken = new CancellationToken();

        var users = new List<UserEntity>();

        userRepositoryMock.Setup(repo => repo.GetUsersAsync(cancellationToken)).ReturnsAsync(users);

        // Act
        var response = await handler.Handle(new GetUsersRequest(), cancellationToken);

        // Assert
        Assert.Empty(response.Users);
    }
}
