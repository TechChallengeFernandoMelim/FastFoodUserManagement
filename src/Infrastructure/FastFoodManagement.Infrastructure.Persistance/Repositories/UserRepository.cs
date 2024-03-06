using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Entities;

namespace FastFoodManagement.Infrastructure.Persistance.Repositories;

public class UserRepository : IUserRepository
{
    public Task AddCustomerAsync(UserEntity customer, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserEntity> GetCustomerByCPFAsync(string identification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserEntity>> GetCustomersAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
