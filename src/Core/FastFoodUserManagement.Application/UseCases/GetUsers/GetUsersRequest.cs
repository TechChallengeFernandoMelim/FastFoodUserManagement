using MediatR;

namespace FastFoodUserManagement.Application.UseCases.GetUsers;

public sealed record GetUsersRequest : IRequest<GetUsersResponse>
{

}
