using FastFoodUserManagement.Application.UseCases;
using FastFoodUserManagement.Controllers;
using FastFoodUserManagement.Domain.Validations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace FastFoodUserManagement.Tests.Controllers;

public class BaseControllerTests
{
    [Fact]
    public async Task Return_NoErrors_ReturnsOk()
    {
        // Arrange
        var validationNotificationsMock = new Mock<IValidationNotifications>();
        validationNotificationsMock.Setup(x => x.HasErrors()).Returns(false);

        var apiBaseResponse = new ApiBaseResponse<string>();
        var baseController = new TestableBaseController(validationNotificationsMock.Object);

        // Act
        var result = await baseController.TestReturn(apiBaseResponse);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        Assert.Equal(apiBaseResponse, okResult.Value);
    }

    [Fact]
    public async Task Return_WithErrors_ReturnsUnprocessableEntity()
    {
        // Arrange
        var validationNotificationsMock = new Mock<IValidationNotifications>();
        validationNotificationsMock.Setup(x => x.HasErrors()).Returns(true);
        validationNotificationsMock.Setup(x => x.GetErrors()).Returns(new Dictionary<string, List<string>>());

        var apiBaseResponse = new ApiBaseResponse<string>();
        var baseController = new TestableBaseController(validationNotificationsMock.Object);

        // Act
        var result = await baseController.TestReturn(apiBaseResponse);

        // Assert
        var unprocessableEntityResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, unprocessableEntityResult.StatusCode);
        Assert.Equal(apiBaseResponse, unprocessableEntityResult.Value);
    }

    [Fact]
    public async Task Return_WithNoStatusCode_ReturnsOk()
    {
        // Arrange
        var validationNotificationsMock = new Mock<IValidationNotifications>();
        validationNotificationsMock.Setup(x => x.HasErrors()).Returns(false);

        var apiBaseResponse = new ApiBaseResponse<string>();
        var baseController = new TestableBaseController(validationNotificationsMock.Object);

        // Act
        var result = await baseController.TestReturn(apiBaseResponse, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        Assert.Equal(apiBaseResponse, okResult.Value);
    }

    [Fact]
    public async Task Return_WithStatusCodeNoContent_ReturnsNoContent()
    {
        // Arrange
        var validationNotificationsMock = new Mock<IValidationNotifications>();
        validationNotificationsMock.Setup(x => x.HasErrors()).Returns(false);

        var apiBaseResponse = new ApiBaseResponse<string>();
        var baseController = new TestableBaseController(validationNotificationsMock.Object);

        // Act
        var result = await baseController.TestReturn(apiBaseResponse, (int)HttpStatusCode.NoContent);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
    }

    // Add more tests for other status codes if needed
}

public class TestableBaseController : BaseController
{
    public TestableBaseController(IValidationNotifications validationNotifications) : base(validationNotifications)
    {
    }

    public Task<IActionResult> TestReturn<T>(ApiBaseResponse<T> apiBaseResponse, int? statusCode = null)
    {
        return Return(apiBaseResponse, statusCode);
    }
}
