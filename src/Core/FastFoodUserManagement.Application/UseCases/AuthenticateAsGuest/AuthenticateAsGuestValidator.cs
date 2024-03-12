using FluentValidation;

namespace FastFoodUserManagement.Application.UseCases.AuthenticateAsGuest;

public class AuthenticateAsGuestValidator : AbstractValidator<AuthenticateAsGuestRequest>
{
    public AuthenticateAsGuestValidator()
    {

    }
}
