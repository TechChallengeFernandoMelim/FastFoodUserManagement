using FluentValidation;

namespace FastFoodUserManagement.Application.UseCases.GetUsers;

public class GetUsersValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersValidator()
    {

    }
}
