using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Entities;
using System.Net;
using System.Text.Json;

namespace FastFoodManagement.Infrastructure.Persistance.Repositories;

public class UserRepository(IAmazonDynamoDB dynamoDb) : IUserRepository
{
    public async Task<bool> AddUserAsync(UserEntity user, CancellationToken cancellationToken)
    {
        var userAsJson = JsonSerializer.Serialize(user);
        var itemAsDocument = Document.FromJson(userAsJson);
        var itemAsAttribute = itemAsDocument.ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = Environment.GetEnvironmentVariable("AWS_TABLE_NAME_DYNAMO"),
            Item = itemAsAttribute
        };

        var response = await dynamoDb.PutItemAsync(createItemRequest, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<UserEntity> GetUserByCPFOrEmailAsync(string identification, string email, CancellationToken cancellationToken)
    {
        var request = new ScanRequest
        {
            TableName = Environment.GetEnvironmentVariable("AWS_TABLE_NAME_DYNAMO"),
            FilterExpression = "identification = :identification OR email = :email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":identification", new AttributeValue { S = identification } },
                { ":email", new AttributeValue { S = email } }
            }
        };

        var response = await dynamoDb.ScanAsync(request, cancellationToken);

        if (response.Items.Count == 0)
            return null;

        var itemAsDocument = Document.FromAttributeMap(response.Items.First());
        return JsonSerializer.Deserialize<UserEntity>(itemAsDocument.ToJson());
    }

    public async Task<IEnumerable<UserEntity>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var request = new ScanRequest
        {
            TableName = Environment.GetEnvironmentVariable("AWS_TABLE_NAME_DYNAMO")
        };

        var response = await dynamoDb.ScanAsync(request, cancellationToken);
        if (response.Items.Count == 0)
            return null;

        var users = response.Items.Select(item =>
        {
            return new UserEntity
            {
                Identification = item.ContainsKey("identification") ? item["identification"].S : null,
                Name = item.ContainsKey("name") ? item["name"].S : null,
                Email = item.ContainsKey("email") ? item["email"].S : null,
                CognitoUserIdentification = item.ContainsKey("clientid") ? item["clientid"].S : null
            };
        });

        return users;
    }
}
