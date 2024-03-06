using AutoMapper;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Exceptions;
using MediatR;

namespace FastFoodUserManagement.Application.UseCases.AuthenticateUser;

public class AuthenticateUserHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<AuthenticateUserRequest, AuthenticateUserResponse>
{
    public async Task<AuthenticateUserResponse> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        var cpf = request.cpf.Replace(".", string.Empty).Replace("-", string.Empty);

        var customerByCpf = await userRepository.GetCustomerByCPFAsync(cpf, cancellationToken)
            ?? throw new ObjectNotFoundException("Usuário não encontrado para esse CPF");

        return mapper.Map<AuthenticateUserResponse>(customerByCpf);
    }
}
