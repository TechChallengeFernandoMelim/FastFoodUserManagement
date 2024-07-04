using MediatR;

namespace FastFoodUserManagement.Application.UseCases.DeleteUserData;

public record DeleteUserDataRequest(string cpf) : IRequest<DeleteUserDataResponse>;
