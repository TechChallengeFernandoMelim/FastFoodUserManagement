using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Domain.Contracts.Repositories;

public interface IUserRepository
{
    Task<bool> AddUserAsync(UserEntity customer, CancellationToken cancellationToken);
    Task<UserEntity> GetUserByCPFAsync(string identification, CancellationToken cancellationToken);
    Task<IEnumerable<UserEntity>> GetUsersAsync(CancellationToken cancellationToken);
}
