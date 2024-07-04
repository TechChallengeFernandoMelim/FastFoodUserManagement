namespace FastFoodUserManagement.Domain.Contracts.Authentication;

public interface IUserDelete
{
    Task DeleteUser(string email);
}
