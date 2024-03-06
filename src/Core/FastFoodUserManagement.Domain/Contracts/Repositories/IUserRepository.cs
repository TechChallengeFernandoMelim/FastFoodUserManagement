using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Domain.Contracts.Repositories;

public interface IUserRepository
{
    Task AddCustomerAsync(UserEntity customer, CancellationToken cancellationToken);
    Task<UserEntity> GetCustomerByCPFAsync(string identification, CancellationToken cancellationToken);
    Task<IEnumerable<UserEntity>> GetCustomersAsync(CancellationToken cancellationToken);
}
