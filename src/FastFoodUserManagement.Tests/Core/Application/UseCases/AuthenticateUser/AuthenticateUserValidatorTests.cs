using FastFoodUserManagement.Application.UseCases.AuthenticateUser;
using FluentValidation.TestHelper;

namespace FastFoodUserManagement.Tests.Application.UseCases.AuthenticateUser
{
    public class AuthenticateUserValidatorTests
    {
        private readonly AuthenticateUserValidator _validator;

        public AuthenticateUserValidatorTests()
        {
            _validator = new AuthenticateUserValidator();
        }

        [Theory]
        [InlineData("123")]
        [InlineData("12345678900")]
        [InlineData("12345678901")]
        [InlineData("123.456.789-01")]
        public void Cpf_InvalidFormat_ShouldHaveError(string cpf)
        {
            // Arrange & Act
            var result = _validator.TestValidate(new AuthenticateUserRequest(cpf));

            // Assert
            result.ShouldHaveValidationErrorFor(request => request.cpf)
                .WithErrorMessage("O cpf deve ser válido");
        }

        [Theory]
        [InlineData("529.982.247-25")]
        [InlineData("52998224725")]
        public void Cpf_ValidFormat_ShouldNotHaveError(string cpf)
        {
            // Arrange & Act
            var result = _validator.TestValidate(new AuthenticateUserRequest(cpf));

            // Assert
            result.ShouldNotHaveValidationErrorFor(request => request.cpf);
        }
    }
}
