using FastFoodUserManagement.Application.UseCases;
using FastFoodUserManagement.Application.UseCases.AuthenticateAsGuest;
using FastFoodUserManagement.Application.UseCases.AuthenticateUser;
using FastFoodUserManagement.Application.UseCases.CreateUser;
using FastFoodUserManagement.Application.UseCases.GetUsers;
using FastFoodUserManagement.Domain.Validations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodUserManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IValidationNotifications validationNotifications, IMediator mediator) : BaseController(validationNotifications)
{
    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <returns>Id of customer created</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<CreateUserResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<CreateUserResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(CreateUserRequest customerCreateRequestDto, CancellationToken cancellationToken)
    {
        var data = await mediator.Send(customerCreateRequestDto, cancellationToken);
        return await Return(new ApiBaseResponse<CreateUserResponse>() { Data = data });
    }

    /// <summary>
    /// Authenticate user.
    /// </summary>
    /// <returns>User with token</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<AuthenticateUserResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<AuthenticateUserResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
    [HttpGet("AuthenticateUser/{cpf}")]
    public async Task<IActionResult> AuthenticateUser(string cpf, CancellationToken cancellationToken)
    {
        var data = await mediator.Send(new AuthenticateUserRequest(cpf), cancellationToken);
        return await Return(new ApiBaseResponse<AuthenticateUserResponse>() { Data = data });
    }

    /// <summary>
    /// Authenticate as guest
    /// </summary>
    /// <returns>Token</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<AuthenticateAsGuestResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<AuthenticateAsGuestResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
    [HttpGet("AuthenticateAsGuest")]
    public async Task<IActionResult> AuthenticateAsGuest(CancellationToken cancellationToken)
    {
        var data = await mediator.Send(new AuthenticateAsGuestRequest(), cancellationToken);
        return await Return(new ApiBaseResponse<AuthenticateAsGuestResponse>() { Data = data });
    }

    /// <summary>
    /// Retrieve a list of all users.
    /// </summary>
    /// <returns>List of users</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<GetUsersResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<GetUsersResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var data = await mediator.Send(new GetUsersRequest(), cancellationToken);
        return await Return(new ApiBaseResponse<GetUsersResponse>() { Data = data });
    }
}
