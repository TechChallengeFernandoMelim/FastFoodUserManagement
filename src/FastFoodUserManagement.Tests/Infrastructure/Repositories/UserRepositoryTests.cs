using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using FastFoodManagement.Infrastructure.Persistance.Repositories;
using FastFoodUserManagement.Domain.Entities;
using Moq;
using System.Net;

namespace FastFoodUserManagement.Tests.Infrastructure.Repositories;

public class UserRepositoryTests
{
    [Fact]
    public async Task AddUserAsync_ValidUser_ReturnsTrue()
    {
        // Arrange
        var dynamoDbMock = new Mock<IAmazonDynamoDB>();
        dynamoDbMock.Setup(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new PutItemResponse { HttpStatusCode = HttpStatusCode.OK });

        var userRepository = new UserRepository(dynamoDbMock.Object);
        var user = new UserEntity { Name = "John", Email = "john@example.com", Identification = "123" };
        var cancellationToken = new CancellationToken();

        // Act
        var result = await userRepository.AddUserAsync(user, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetUserByCPFOrEmailAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var dynamoDbMock = new Mock<IAmazonDynamoDB>();
        dynamoDbMock.Setup(x => x.ScanAsync(It.IsAny<ScanRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new ScanResponse
                    {
                        Items = new List<Dictionary<string, AttributeValue>>
                        {
                                new Dictionary<string, AttributeValue>
                                {
                                    { "identification", new AttributeValue { S = "123" } },
                                    { "name", new AttributeValue { S = "John" } },
                                    { "email", new AttributeValue { S = "john@example.com" } }
                                }
                        }
                    });

        var userRepository = new UserRepository(dynamoDbMock.Object);
        var cancellationToken = new CancellationToken();

        // Act
        var result = await userRepository.GetUserByCPFOrEmailAsync("123", "john@example.com", cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.Name);
        Assert.Equal("john@example.com", result.Email);
    }

    [Fact]
    public async Task GetUsersAsync_UsersExist_ReturnsListOfUsers()
    {
        // Arrange
        var dynamoDbMock = new Mock<IAmazonDynamoDB>();
        dynamoDbMock.Setup(x => x.ScanAsync(It.IsAny<ScanRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new ScanResponse
                    {
                        Items = new List<Dictionary<string, AttributeValue>>
                        {
                                new Dictionary<string, AttributeValue>
                                {
                                    { "identification", new AttributeValue { S = "123" } },
                                    { "name", new AttributeValue { S = "John" } },
                                    { "email", new AttributeValue { S = "john@example.com" } }
                                },
                                new Dictionary<string, AttributeValue>
                                {
                                    { "identification", new AttributeValue { S = "456" } },
                                    { "name", new AttributeValue { S = "Jane" } },
                                    { "email", new AttributeValue { S = "jane@example.com" } }
                                }
                        }
                    });

        var userRepository = new UserRepository(dynamoDbMock.Object);
        var cancellationToken = new CancellationToken();

        // Act
        var result = await userRepository.GetUsersAsync(cancellationToken);

        // Assert
        Assert.NotNull(result);
        var userList = result.ToList();
        Assert.Equal(2, userList.Count);

        var user1 = userList[0];
        Assert.Equal("123", user1.Identification);
        Assert.Equal("John", user1.Name);
        Assert.Equal("john@example.com", user1.Email);

        var user2 = userList[1];
        Assert.Equal("456", user2.Identification);
        Assert.Equal("Jane", user2.Name);
        Assert.Equal("jane@example.com", user2.Email);
    }
}
