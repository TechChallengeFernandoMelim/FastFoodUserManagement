using MediatR;

namespace FastFoodUserManagement.Application.UseCases.AuthenticateAsGuest;

public sealed record AuthenticateAsGuestRequest : IRequest<AuthenticateAsGuestResponse>
{
}
