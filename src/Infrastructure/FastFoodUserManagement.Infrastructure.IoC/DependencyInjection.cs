using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.SQS;
using FastFoodManagement.Infrastructure.Persistance.Repositories;
using FastFoodUserManagement.Application.UseCases;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Contracts.Logger;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Validations;
using FastFoodUserManagement.Infrastructure.Cognito.Authentication;
using FastFoodUserManagement.Infrastructure.Cognito.Creation;
using FastFoodUserManagement.Infrastructure.Cognito.Delete;
using FastFoodUserManagement.Infrastructure.SQS.Logger;
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
        ConfigureCognito(services);
        ConfigureRepositories(services);
        ConfigureDatabase(services);
        ConfigureNotificationServices(services);
        ConfigureValidators(services);
        ConfigureMediatr(services);
        ConfigureAutomapper(services);
        ConfigureSQS(services);
    }

    private static void ConfigureSQS(IServiceCollection services)
    {
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_DYNAMO");
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY_DYNAMO");

        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
        var sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.USEast1);

        services.AddSingleton(sqsClient);
        services.AddSingleton<ILogger, Logger>();
    }

    private static void ConfigureCognito(IServiceCollection services)
    {
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_DYNAMO");
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY_DYNAMO");

        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

        var config = new AmazonCognitoIdentityProviderConfig();

        var cognitoProvider = new AmazonCognitoIdentityProviderClient(credentials, Amazon.RegionEndpoint.USEast1);

        services.AddSingleton(cognitoProvider);
        services.AddSingleton<IUserCreation, CognitoUserCreation>();
        services.AddSingleton<IUserAuthentication, CognitoUserAuthentication>();
        services.AddSingleton<IUserDelete, CognitoDeleteUser>();
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

