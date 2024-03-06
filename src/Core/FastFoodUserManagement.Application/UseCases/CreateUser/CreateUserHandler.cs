using AutoMapper;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Entities;
using FastFoodUserManagement.Domain.Validations;
using MediatR;

namespace FastFoodUserManagement.Application.UseCases.CreateUser;

public class CreateUserHandler(IUserRepository userRepository, IMapper mapper, IValidationNotifications validationNotifications) : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<UserEntity>(request);

        var existingCustomer = await userRepository.GetUserByCPFAsync(customer.Identification, cancellationToken);

        if (existingCustomer != null)
            validationNotifications.AddError("Identification", "Já existe um usuário cadastrado com esse CPF.");
        else
        {
            customer.Identification = customer.Identification.Replace(".", string.Empty).Replace("-", string.Empty);
            await userRepository.AddUserAsync(customer, cancellationToken);
        }

        return new CreateUserResponse();
    }
}
