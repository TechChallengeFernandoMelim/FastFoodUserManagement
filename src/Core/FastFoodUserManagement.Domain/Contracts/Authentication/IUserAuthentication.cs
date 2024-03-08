using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Domain.Contracts.Authentication;

public interface IUserAuthentication
{
    Task<string> AuthenticateUser(UserEntity user, CancellationToken cancellationToken);
}
