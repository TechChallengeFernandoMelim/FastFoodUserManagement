namespace FastFoodUserManagement.Domain.Contracts.Logger;

public interface ILogger
{
    Task Log(string stackTrace, string message, string exception);
}
