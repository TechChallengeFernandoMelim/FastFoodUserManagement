using FastFoodUserManagement.Domain.Exceptions;

namespace FastFoodUserManagement.Tests.Core.Domain;

public class ObjectNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithoutParameters_CreatesObjectNotFoundException()
    {
        // Arrange & Act
        var exception = new ObjectNotFoundException();

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public void Constructor_WithMessage_CreatesObjectNotFoundExceptionWithMessage()
    {
        // Arrange
        var errorMessage = "Object not found";

        // Act
        var exception = new ObjectNotFoundException(errorMessage);

        // Assert
        Assert.Equal(errorMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesObjectNotFoundExceptionWithMessageAndInnerException()
    {
        // Arrange
        var errorMessage = "Object not found";
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new ObjectNotFoundException(errorMessage, innerException);

        // Assert
        Assert.Equal(errorMessage, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
