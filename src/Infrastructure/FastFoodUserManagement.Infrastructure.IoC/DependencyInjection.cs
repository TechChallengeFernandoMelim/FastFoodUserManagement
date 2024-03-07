using Amazon.DynamoDBv2;
using Amazon.Runtime;
using FastFoodManagement.Infrastructure.Persistance.Repositories;
using FastFoodUserManagement.Application.UseCases;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Validations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.AWS.Logger;
using NLog.Config;
using System.Reflection;

namespace FastFoodUserManagement.Infrastructure.IoC;

public static class DependencyInjection
{
    private static string pathToApplicationAssembly = Path.Combine(AppContext.BaseDirectory, "FastFoodUserManagement.Application.dll");

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureLogging(services);
        ConfigureRepositories(services);
        ConfigureDatabase(services);
        ConfigureNotificationServices(services);
        ConfigureValidators(services);
        ConfigureMediatr(services);
        ConfigureAutomapper(services);
    }

    private static void ConfigureLogging(IServiceCollection services)
    {
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_DYNAMO");
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY_DYNAMO");

        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

        var config = new LoggingConfiguration();

        config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, new AWSTarget()
        {
            LogGroup = Environment.GetEnvironmentVariable("LOG_GROUP"),
            Region = Environment.GetEnvironmentVariable("LOG_REGION"),
            Credentials = credentials
        });


        LogManager.Configuration = config;

        var log = LogManager.GetCurrentClassLogger();

        services.AddSingleton<Logger>(log);
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

    private static void ConfigureDatabase(IServiceCollection services)
    {
        var clientConfig = new AmazonDynamoDBConfig();
        clientConfig.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_DYNAMO");
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY_DYNAMO");

        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

        services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(credentials, clientConfig));
    }

    private static void ConfigureRepositories(IServiceCollection services)
        => services.AddScoped<IUserRepository, UserRepository>();

    private static void ConfigureNotificationServices(IServiceCollection services)
        => services.AddScoped<IValidationNotifications, ValidationNotifications>();


    private static void ConfigureValidators(IServiceCollection services)
        => services.AddValidatorsFromAssembly(Assembly.LoadFrom(pathToApplicationAssembly));
}

