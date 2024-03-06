using MediatR;

namespace FastFoodUserManagement.Application.UseCases.AuthenticateUser;

public sealed record AuthenticateUserRequest(string cpf) :
 IRequest<AuthenticateUserResponse>;
