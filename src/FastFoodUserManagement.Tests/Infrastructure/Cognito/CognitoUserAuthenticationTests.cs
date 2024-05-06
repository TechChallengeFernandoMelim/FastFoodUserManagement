using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using FastFoodUserManagement.Domain.Entities;
using FastFoodUserManagement.Infrastructure.Cognito.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.Runtime.Internal.Util;
using System.Collections.Specialized;
using Amazon.Runtime;

namespace FastFoodUserManagement.Tests.Infrastructure.Cognito;

public class CognitoUserAuthenticationTests
{
    AWSCredentials credentials = new BasicAWSCredentials("trest", "test");

    [Fact]
    public async Task AuthenticateUser_ValidUser_ReturnsToken()
    {
        // Arrange
        var cognitoMock = new Mock<AmazonCognitoIdentityProviderClient>(credentials, Amazon.RegionEndpoint.USEast1);
        var cacheMock = new Mock<IMemoryCache>();
        var mockCacheEntry = new Mock<ICacheEntry>();

        var user = new UserEntity { Email = "test@example.com", Identification = "password123" };
        var cancellationToken = new CancellationToken();
        var expectedToken = "testToken";
        var expectedUserPoolId = "testUserPoolId";
        var expectedClientId = "testClientId";

        string? keyPayload = null;
        cacheMock
        .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
        .Callback((object k) => keyPayload = (string)k)
        .Returns(mockCacheEntry.Object);

        object? valuePayload = null;
        mockCacheEntry
            .SetupSet(mce => mce.Value = It.IsAny<object>())
            .Callback<object>(v => valuePayload = v);

        TimeSpan? expirationPayload = null;
        mockCacheEntry
            .SetupSet(mce => mce.AbsoluteExpirationRelativeToNow = It.IsAny<TimeSpan?>())
            .Callback<TimeSpan?>(dto => expirationPayload = dto);

        Environment.SetEnvironmentVariable("AWS_USER_POOL_ID", expectedUserPoolId);
        Environment.SetEnvironmentVariable("AWS_CLIENT_ID_COGNITO", expectedClientId);

        cognitoMock.Setup(x => x.AdminInitiateAuthAsync(It.IsAny<AdminInitiateAuthRequest>(), cancellationToken))
                   .ReturnsAsync(new AdminInitiateAuthResponse { AuthenticationResult = new AuthenticationResultType { IdToken = expectedToken } });

        var userAuthentication = new CognitoUserAuthentication(cognitoMock.Object, cacheMock.Object);

        // Act
        var result = await userAuthentication.AuthenticateUser(user, cancellationToken);

        // Assert
        Assert.Equal(expectedToken, result);
        //cacheMock.Verify(x => x.Set(user.Identification, expectedToken, TimeSpan.FromMinutes(30)), Times.Once);
    }

    [Fact]
    public async Task AuthenticateUser_CachedTokenExists_ReturnsCachedToken()
    {
        // Arrange
        var cognitoMock = new Mock<AmazonCognitoIdentityProviderClient>(credentials, Amazon.RegionEndpoint.USEast1);

        var cache = new MemoryCache(new MemoryCacheOptions());

        var user = new UserEntity { Email = "test@example.com", Identification = "password123" };
        var cancellationToken = new CancellationToken();
        var cachedToken = "cachedToken";

        cache.Set(user.Identification, cachedToken, TimeSpan.FromMinutes(30));

        var userAuthentication = new CognitoUserAuthentication(cognitoMock.Object, cache);

        // Act
        var result = await userAuthentication.AuthenticateUser(user, cancellationToken);

        // Assert
        Assert.Equal(cachedToken, result);
    }
}
