using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Domain.Contracts.Authentication;

public interface IUserCreation
{
    Task<string> CreateUser(UserEntity user, CancellationToken cancellationToken);
}
