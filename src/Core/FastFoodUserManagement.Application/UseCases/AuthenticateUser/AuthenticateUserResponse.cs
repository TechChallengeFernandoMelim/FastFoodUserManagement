namespace FastFoodUserManagement.Application.UseCases.AuthenticateUser;

public sealed record AuthenticateUserResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
