using AutoMapper;
using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Application.UseCases.GetUsers;

public class GetUsersMapper : Profile
{
    public GetUsersMapper()
    {
        CreateMap<UserEntity, User>();
    }
}
