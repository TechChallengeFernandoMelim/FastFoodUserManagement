namespace FastFoodUserManagement.Application.UseCases.AuthenticateAsGuest;

public sealed record AuthenticateAsGuestResponse
{
    public string Token { get; set; }
}
