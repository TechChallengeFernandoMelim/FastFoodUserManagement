using AutoMapper;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using MediatR;

namespace FastFoodUserManagement.Application.UseCases.GetUsers;

public class GetUsersHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var customers = await userRepository.GetUsersAsync(cancellationToken);

        var customersDto = mapper.Map<IEnumerable<User>>(customers);

        return new GetUsersResponse() { Users = customersDto };
    }
}
