using System.Text.Json.Serialization;

namespace FastFoodUserManagement.Domain.Entities;

public class UserEntity
{
    [JsonPropertyName("pk")]
    public string Pk => Id;

    [JsonPropertyName("sk")]
    public string Sk => Pk;

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("identification")]
    public string Identification { get; set; }
}
