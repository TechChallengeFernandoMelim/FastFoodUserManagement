using System.Text.Json.Serialization;

namespace FastFoodUserManagement.Domain.Entities;

public class UserEntity
{
    [JsonPropertyName("pk")]
    public string Pk => Identification;

    [JsonPropertyName("sk")]
    public string Sk => Pk;

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("identification")]
    public string Identification { get; set; }

    [JsonPropertyName("clientid")]
    public string ClientId { get; set; }
}
