﻿namespace FastFoodUserManagement.Application.UseCases.GetUsers;

public sealed record GetUsersResponse
{
    public IEnumerable<User> Users { get; set; }
}

public record User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Identification { get; set; }
}