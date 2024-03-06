using AutoMapper;
using FastFoodUserManagement.Domain.Entities;

namespace FastFoodUserManagement.Application.UseCases.AuthenticateUser;

public class AuthenticateUserMapper: Profile
{
    public AuthenticateUserMapper()
    {
        CreateMap<UserEntity, AuthenticateUserResponse>();
    }
}
