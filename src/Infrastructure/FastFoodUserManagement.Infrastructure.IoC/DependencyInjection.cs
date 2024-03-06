using FastFoodManagement.Infrastructure.Persistance.Repositories;
using FastFoodUserManagement.Application.UseCases;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Validations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FastFoodUserManagement.Infrastructure.IoC;

public static class DependencyInjection
{
    private static string pathToApplicationAssembly = Path.Combine(AppContext.BaseDirectory, "FastFoodUserManagement.Application.dll");

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureRepositories(services);
        ConfigureDatabase(services, configuration);
        ConfigureNotificationServices(services);
        ConfigureValidators(services);
        ConfigureMediatr(services);
        ConfigureAutomapper(services);
    }

    private static void ConfigureAutomapper(IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.LoadFrom(pathToApplicationAssembly));
    }

    private static void ConfigureMediatr(IServiceCollection services)
    {


        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.LoadFrom(pathToApplicationAssembly)));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {

    }

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void ConfigureNotificationServices(IServiceCollection services)
    =>
        services.AddScoped<IValidationNotifications, ValidationNotifications>();
    

    private static void ConfigureValidators(IServiceCollection services)
       => services.AddValidatorsFromAssembly(Assembly.LoadFrom(pathToApplicationAssembly));
}

