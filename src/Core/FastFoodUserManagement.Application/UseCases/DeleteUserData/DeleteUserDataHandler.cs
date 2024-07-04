using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Exceptions;
using FastFoodUserManagement.Domain.Validations;
using MediatR;

namespace FastFoodUserManagement.Application.UseCases.DeleteUserData;

public class DeleteUserDataHandler(IUserRepository userRepository, IValidationNotifications validationNotifications, IUserDelete userDelete) : IRequestHandler<DeleteUserDataRequest, DeleteUserDataResponse>
{
    public async Task<DeleteUserDataResponse> Handle(DeleteUserDataRequest request, CancellationToken cancellationToken)
    {
        var cpf = request.cpf.Replace(".", "").Replace("-", "");

        var user = await userRepository.GetUserByCPFOrEmailAsync(cpf, string.Empty, cancellationToken)
            ?? throw new ObjectNotFoundException("Usuário não encontrado para esse CPF");

        await userRepository.DeleteUserData(cpf, cancellationToken);
        await userDelete.DeleteUser(user.Email);

        return new DeleteUserDataResponse();
    }
}
