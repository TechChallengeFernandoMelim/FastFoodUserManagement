using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace FastFoodManagement.Infrastructure.Persistance.Repositories;

public class UserRepository(IAmazonDynamoDB dynamoDb, IOptions<DatabaseSettings> options) : IUserRepository
{
    public async Task<bool> AddUserAsync(UserEntity user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid().ToString();
        var userAsJson = JsonSerializer.Serialize(user);
        var itemAsDocument = Document.FromJson(userAsJson);
        var itemAsAttribute = itemAsDocument.ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = options.Value.TableName,
            Item = itemAsAttribute
        };

        var response = await dynamoDb.PutItemAsync(createItemRequest, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<UserEntity> GetUserByCPFAsync(string identification, CancellationToken cancellationToken)
    {
        var request = new ScanRequest
        {
            TableName = options.Value.TableName,
            FilterExpression = "identification = :identification",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":identification", new AttributeValue { S = identification } }
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
            TableName = options.Value.TableName
        };

        var response = await dynamoDb.ScanAsync(request, cancellationToken);
        if (response.Items.Count == 0)
            return null;

        var users = response.Items.Select(item =>
        {
            var user = new UserEntity
            {
                Id = item.ContainsKey("id") ? item["id"].S : null,
                Identification = item.ContainsKey("identification") ? item["identification"].S : null,
                Name = item.ContainsKey("name") ? item["name"].S : null,
                Email = item.ContainsKey("email") ? item["email"].S : null
            };
            return user;
        });

        return users;
    }
}
