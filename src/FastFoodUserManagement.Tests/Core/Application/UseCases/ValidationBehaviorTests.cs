using FastFoodUserManagement.Application.UseCases;
using FastFoodUserManagement.Domain.Validations;
using FluentValidation;
using MediatR;
using Moq;

namespace FastFoodUserManagement.Tests.Core.Application.UseCases;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WithoutValidator_CallsNextHandler()
    {
        // Arrange
        var nextHandlerMock = new Mock<RequestHandlerDelegate<MyResponse>>();
        var validationNotificationsMock = new Mock<IValidationNotifications>();
        var behavior = new ValidationBehavior<MyRequest, MyResponse>(null, validationNotificationsMock.Object);
        var request = new MyRequest();
        var cancellationToken = new CancellationToken();

        // Act
        await behavior.Handle(request, nextHandlerMock.Object, cancellationToken);

        // Assert
        nextHandlerMock.Verify(next => next(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<MyRequest>>();
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Property", "Error message")
            });
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<MyRequest>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

        var validationNotificationsMock = new Mock<IValidationNotifications>();
        var behavior = new ValidationBehavior<MyRequest, MyResponse>(validatorMock.Object, validationNotificationsMock.Object);
        var request = new MyRequest();
        var cancellationToken = new CancellationToken();

        // Act & Assert
        await Assert.ThrowsAsync<FastFoodUserManagement.Domain.Exceptions.ValidationException>(() => behavior.Handle(request, null, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_AddsErrorsToValidationNotifications()
    {
        // Arrange
        var validatorMock = new Mock<IValidator<MyRequest>>();
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Property", "Error message")
            });
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<MyRequest>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

        var validationNotificationsMock = new Mock<IValidationNotifications>();
        var behavior = new ValidationBehavior<MyRequest, MyResponse>(validatorMock.Object, validationNotificationsMock.Object);
        var request = new MyRequest();
        var cancellationToken = new CancellationToken();

        // Act
        await Assert.ThrowsAsync<FastFoodUserManagement.Domain.Exceptions.ValidationException>(() => behavior.Handle(request, null, cancellationToken));

        // Assert
        validationNotificationsMock.Verify(vn => vn.AddError("Property", "Error message"), Times.Once);
    }

    public class MyRequest : IRequest<MyResponse> { }

    public class MyResponse { }
}



