using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using System.Net;

namespace FastFoodUserManagement.Infrastructure.Cognito.Delete;

public class CognitoDeleteUser(AmazonCognitoIdentityProviderClient cognito) : IUserDelete
{
    public async Task DeleteUser(string email)
    {
        var userPoolId = Environment.GetEnvironmentVariable("AWS_USER_POOL_ID");

        var deleteUserRequest = new AdminDeleteUserRequest
        {
            UserPoolId = userPoolId,
            Username = email
        };

        try
        {
            var response = await cognito.AdminDeleteUserAsync(deleteUserRequest);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to delete user with email {email} from the Cognito user pool.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while trying to delete the user: {ex.Message}");
        }
    }
}
