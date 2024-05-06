using FastFoodUserManagement.Application.UseCases.CreateUser;
using FluentValidation.TestHelper;

namespace FastFoodUserManagement.Tests.Core.Application.UseCases.CreateUser;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator;

    public CreateUserValidatorTests()
    {
        _validator = new CreateUserValidator();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("ab")]
    public void Name_Invalid_ShouldHaveError(string name)
    {
        // Arrange & Act
        var result = _validator.TestValidate(new CreateUserRequest(name, "test@example.com", "12345678900"));
        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Name);
    }

    [Theory]
    [InlineData("John Doe")]
    [InlineData("John Doe Jr.")]
    public void Name_Valid_ShouldNotHaveError(string name)
    {
        // Arrange & Act
        var result = _validator.TestValidate(new CreateUserRequest(name, "test@example.com", "12345678900"));

        // Assert
        result.ShouldNotHaveValidationErrorFor(request => request.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    public void Email_Invalid_ShouldHaveError(string email)
    {
        // Arrange & Act
        var result = _validator.TestValidate(new CreateUserRequest("John Doe", email, "12345678900"));

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Email);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("john.doe@example.com")]
    public void Email_Valid_ShouldNotHaveError(string email)
    {
        // Arrange & Act
        var result = _validator.TestValidate(new CreateUserRequest("John Doe", email, "12345678900"));

        // Assert
        result.ShouldNotHaveValidationErrorFor(request => request.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    [InlineData("12345678901")]
    [InlineData("123.456.789-01")]
    public void Identification_Invalid_ShouldHaveError(string cpf)
    {
        // Arrange & Act
        var result = _validator.TestValidate(new CreateUserRequest("John Doe", "test@example.com", cpf));

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Identification);
    }

    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    public void Identification_Valid_ShouldNotHaveError(string cpf)
    {
        // Arrange & Act
        var result = _validator.TestValidate(new CreateUserRequest("John Doe", "test@example.com", cpf));

        // Assert
        result.ShouldNotHaveValidationErrorFor(request => request.Identification);
    }
}
