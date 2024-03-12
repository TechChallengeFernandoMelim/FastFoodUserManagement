using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Entities;
using MediatR;

namespace FastFoodUserManagement.Application.UseCases.AuthenticateAsGuest;

public class AuthenticateAsGuestHandler(IUserAuthentication userAuthentication) : IRequestHandler<AuthenticateAsGuestRequest, AuthenticateAsGuestResponse>
{
    public async Task<AuthenticateAsGuestResponse> Handle(AuthenticateAsGuestRequest request, CancellationToken cancellationToken)
    {
        var user = new UserEntity()
        {
            Email = Environment.GetEnvironmentVariable("GUEST_EMAIL"),
            Identification = Environment.GetEnvironmentVariable("GUEST_IDENTIFICATION")
        };

        var response = new AuthenticateAsGuestResponse
        {
            Token = await userAuthentication.AuthenticateUser(user, cancellationToken)
        };

        return response;
    }
}
