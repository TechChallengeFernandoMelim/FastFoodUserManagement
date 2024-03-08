namespace FastFoodUserManagement.Application.UseCases.AuthenticateUser;

public sealed record AuthenticateUserResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Identification { get; set; }
    public string Token { get; set; }
}
