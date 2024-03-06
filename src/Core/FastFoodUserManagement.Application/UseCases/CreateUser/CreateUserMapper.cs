using AutoMapper;
using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Application.UseCases.CreateUser;

public class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateUserRequest, UserEntity>();
    }
}
