using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Domain.Contracts.Authentication;

public interface IUserCreation
{
    Task CreateUser(UserEntity user, CancellationToken cancellationToken);
}
