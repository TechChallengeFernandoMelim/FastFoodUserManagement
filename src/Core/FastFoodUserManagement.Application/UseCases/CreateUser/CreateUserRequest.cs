using MediatR;

namespace FastFoodUserManagement.Application.UseCases.CreateUser;

public sealed record CreateUserRequest(
    string Name, string Email, string Identification) :
     IRequest<CreateUserResponse>;
