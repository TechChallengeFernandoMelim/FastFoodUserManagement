using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Infrastructure.Cognito.Creation;

public class CognitoUserCreation(AmazonCognitoIdentityProviderClient cognito) : IUserCreation
{


    public async Task CreateUser(UserEntity user, CancellationToken cancellationToken)
    {
        var userPoolId = Environment.GetEnvironmentVariable("AWS_USER_POOL_ID");

        var request = new AdminCreateUserRequest()
        {
            UserPoolId = userPoolId,
            Username = user.Email
        };

        var response = await cognito.AdminCreateUserAsync(request, cancellationToken);

        var setPassword = new AdminSetUserPasswordRequest()
        {
            Password = user.Identification,
            Username = user.Email,
            UserPoolId = userPoolId,
            Permanent = true
        };

        await cognito.AdminSetUserPasswordAsync(setPassword, cancellationToken);
    }
}
