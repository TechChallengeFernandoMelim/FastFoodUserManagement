using FastFoodUserManagement.Application.UseCases;
using FastFoodUserManagement.Application.UseCases.AuthenticateAsGuest;
using FastFoodUserManagement.Application.UseCases.AuthenticateUser;
using FastFoodUserManagement.Application.UseCases.CreateUser;
using FastFoodUserManagement.Controllers;
using FastFoodUserManagement.Domain.Validations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FastFoodUserManagement.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task CreateUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var validationNotificationsMock = new Mock<IValidationNotifications>();

            var controller = new UserController(validationNotificationsMock.Object, mediatorMock.Object);
            var request = new CreateUserRequest("John Doe", "john@example.com", "123456789");
            var cancellationToken = new CancellationToken();

            mediatorMock.Setup(x => x.Send(request, cancellationToken)).ReturnsAsync(new CreateUserResponse());

            // Act
            var result = await controller.CreateUser(request, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<ApiBaseResponse<CreateUserResponse>>(okResult.Value);
        }

        [Fact]
        public async Task CreateUser_InvalidRequest_ReturnsUnprocessableEntity()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var validationNotificationsMock = new Mock<IValidationNotifications>();

            validationNotificationsMock.Setup(x => x.HasErrors()).Returns(true);
            validationNotificationsMock.Setup(x => x.GetErrors()).Returns(new Dictionary<string, List<string>>());

            var controller = new UserController(validationNotificationsMock.Object, mediatorMock.Object);
            var request = new CreateUserRequest("John Doe", "", "123456789");
            var cancellationToken = new CancellationToken();

            // Act
            var result = await controller.CreateUser(request, cancellationToken);

            // Assert
            var unprocessableEntityResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
            Assert.IsType<ApiBaseResponse<CreateUserResponse>>(unprocessableEntityResult.Value);
        }

        [Fact]
        public async Task AuthenticateUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var validationNotificationsMock = new Mock<IValidationNotifications>();

            var controller = new UserController(validationNotificationsMock.Object, mediatorMock.Object);
            var cpf = "12345678900"; // CPF válido
            var cancellationToken = new CancellationToken();

            mediatorMock.Setup(x => x.Send(It.IsAny<AuthenticateUserRequest>(), cancellationToken)).ReturnsAsync(new AuthenticateUserResponse());

            // Act
            var result = await controller.AuthenticateUser(cpf, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<ApiBaseResponse<AuthenticateUserResponse>>(okResult.Value);
        }

        [Fact]
        public async Task AuthenticateAsGuest_ValidRequest_ReturnsOk()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var validationNotificationsMock = new Mock<IValidationNotifications>();

            var controller = new UserController(validationNotificationsMock.Object, mediatorMock.Object);
            var cancellationToken = new CancellationToken();

            mediatorMock.Setup(x => x.Send(It.IsAny<AuthenticateAsGuestRequest>(), cancellationToken)).ReturnsAsync(new AuthenticateAsGuestResponse());

            // Act
            var result = await controller.AuthenticateAsGuest(cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<ApiBaseResponse<AuthenticateAsGuestResponse>>(okResult.Value);
        }
    }
}
