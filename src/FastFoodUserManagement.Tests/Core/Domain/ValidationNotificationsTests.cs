using FastFoodUserManagement.Domain.Validations;

namespace FastFoodUserManagement.Tests.Core.Domain;

public class ValidationNotificationsTests
{
    [Fact]
    public void AddError_NewKey_AddsErrorToList()
    {
        // Arrange
        var validationNotifications = new ValidationNotifications();
        var key = "Key";
        var error = "Error";

        // Act
        validationNotifications.AddError(key, error);

        // Assert
        Assert.True(validationNotifications.HasErrors());
        var errors = validationNotifications.GetErrors();
        Assert.Single(errors);
        Assert.True(errors.ContainsKey(key));
        Assert.Single(errors[key]);
        Assert.Equal(error, errors[key][0]);
    }

    [Fact]
    public void AddError_ExistingKey_AddsErrorToList()
    {
        // Arrange
        var validationNotifications = new ValidationNotifications();
        var key = "Key";
        var error1 = "Error1";
        var error2 = "Error2";

        // Act
        validationNotifications.AddError(key, error1);
        validationNotifications.AddError(key, error2);

        // Assert
        Assert.True(validationNotifications.HasErrors());
        var errors = validationNotifications.GetErrors();
        Assert.Single(errors);
        Assert.True(errors.ContainsKey(key));
        Assert.Equal(2, errors[key].Count);
        Assert.Equal(error1, errors[key][0]);
        Assert.Equal(error2, errors[key][1]);
    }

    [Fact]
    public void HasErrors_NoErrors_ReturnsFalse()
    {
        // Arrange
        var validationNotifications = new ValidationNotifications();

        // Assert
        Assert.False(validationNotifications.HasErrors());
    }

    [Fact]
    public void HasErrors_WithErrors_ReturnsTrue()
    {
        // Arrange
        var validationNotifications = new ValidationNotifications();
        validationNotifications.AddError("Key", "Error");

        // Assert
        Assert.True(validationNotifications.HasErrors());
    }

    [Fact]
    public void GetErrors_NoErrors_ReturnsEmptyDictionary()
    {
        // Arrange
        var validationNotifications = new ValidationNotifications();

        // Act
        var errors = validationNotifications.GetErrors();

        // Assert
        Assert.NotNull(errors);
        Assert.Empty(errors);
    }

    [Fact]
    public void GetErrors_WithErrors_ReturnsDictionaryWithErrors()
    {
        // Arrange
        var validationNotifications = new ValidationNotifications();
        validationNotifications.AddError("Key1", "Error1");
        validationNotifications.AddError("Key2", "Error2");

        // Act
        var errors = validationNotifications.GetErrors();

        // Assert
        Assert.NotNull(errors);
        Assert.Equal(2, errors.Count);
        Assert.True(errors.ContainsKey("Key1"));
        Assert.True(errors.ContainsKey("Key2"));
        Assert.Single(errors["Key1"]);
        Assert.Single(errors["Key2"]);
        Assert.Equal("Error1", errors["Key1"][0]);
        Assert.Equal("Error2", errors["Key2"][0]);
    }
}
