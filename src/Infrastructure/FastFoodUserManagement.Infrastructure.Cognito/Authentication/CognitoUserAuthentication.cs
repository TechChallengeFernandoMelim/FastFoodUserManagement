﻿using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace FastFoodUserManagement.Infrastructure.Cognito.Authentication;

public class CognitoUserAuthentication(AmazonCognitoIdentityProviderClient cognito) : IUserAuthentication
{
    public async Task<string> AuthenticateUser(UserEntity user, CancellationToken cancellationToken)
    {
        var userPoolId = Environment.GetEnvironmentVariable("AWS_USER_POOL_ID");
        var clientId = Environment.GetEnvironmentVariable("AWS_CLIENT_ID_COGNITO");

        var authParameters = new Dictionary<string, string>
        {
            { "USERNAME", user.Email },
            { "PASSWORD", user.Identification }
        };

        var request = new AdminInitiateAuthRequest()
        {
            AuthParameters = authParameters,
            ClientId = clientId,
            AuthFlow = "ADMIN_USER_PASSWORD_AUTH",
            UserPoolId = userPoolId
        };

        var response = await cognito.AdminInitiateAuthAsync(request, cancellationToken);
        return response.AuthenticationResult.IdToken;
    }
}
