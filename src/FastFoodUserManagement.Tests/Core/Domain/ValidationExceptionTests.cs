using FastFoodUserManagement.Domain.Exceptions;
using FastFoodUserManagement.Domain.Validations;

namespace FastFoodUserManagement.Tests.Core.Domain;

public class ValidationExceptionTests
{
    [Fact]
    public void Constructor_NullValidationNotifications_ThrowsArgumentNullException()
    {
        // Arrange, Act, Assert
        Assert.Throws<ArgumentNullException>(() => new ValidationException(null));
    }

    [Fact]
    public void Constructor_ValidValidationNotifications_CreatesInstance()
    {
        // Arrange
        var validationNotifications = new ValidationNotifications();

        // Act
        var exception = new ValidationException(validationNotifications);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(validationNotifications, exception.ValidationNotifications);
    }
}
