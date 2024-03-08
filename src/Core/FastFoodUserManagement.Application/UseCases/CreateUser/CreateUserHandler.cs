using AutoMapper;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Entities;
using FastFoodUserManagement.Domain.Validations;
using MediatR;

namespace FastFoodUserManagement.Application.UseCases.CreateUser;

public class CreateUserHandler(
    IUserRepository userRepository, 
    IMapper mapper, IValidationNotifications 
    validationNotifications, 
    IUserCreation userCreation) : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<UserEntity>(request);
        user.Identification = user.Identification.Replace(".", string.Empty).Replace("-", string.Empty);

        var existingCustomer = await userRepository.GetUserByCPFAsync(user.Identification, cancellationToken);

        if (existingCustomer != null)
            validationNotifications.AddError("Identification", "Já existe um usuário cadastrado com esse CPF.");
        else
        {
            var clientId = await userCreation.CreateUser(user, cancellationToken);
            user.ClientId = clientId;
            await userRepository.AddUserAsync(user, cancellationToken);
        }

        return new CreateUserResponse();
    }
}
