using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Domain.Contracts.Repositories;

public interface IUserRepository
{
    Task<bool> AddUserAsync(UserEntity customer, CancellationToken cancellationToken);
    Task<UserEntity> GetUserByCPFOrEmailAsync(string identification, string email, CancellationToken cancellationToken);
    Task<IEnumerable<UserEntity>> GetUsersAsync(CancellationToken cancellationToken);
    Task DeleteUserData(string cpf, CancellationToken cancellationToken);
}
