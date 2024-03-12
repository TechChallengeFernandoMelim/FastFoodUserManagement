using FastFoodManagement.Infrastructure.Persistance;
using FastFoodUserManagement.Infrastructure.IoC;
using FastFoodUserManagement.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();



app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();
