#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/FastFoodUserManagement/FastFoodUserManagement.csproj", "src/Presentation/FastFoodUserManagement/"]
COPY ["src/Infrastructure/FastFoodUserManagement.Infrastructure.IoC/FastFoodUserManagement.Infrastructure.IoC.csproj", "src/Infrastructure/FastFoodUserManagement.Infrastructure.IoC/"]
COPY ["src/Core/FastFoodUserManagement.Application/FastFoodUserManagement.Application.csproj", "src/Core/FastFoodUserManagement.Application/"]
COPY ["src/Core/FastFoodUserManagement.Domain/FastFoodUserManagement.Domain.csproj", "src/Core/FastFoodUserManagement.Domain/"]
COPY ["src/Infrastructure/FastFoodManagement.Infrastructure.Persistance/FastFoodManagement.Infrastructure.Persistance.csproj", "src/Infrastructure/FastFoodManagement.Infrastructure.Persistance/"]
RUN dotnet restore "./src/Presentation/FastFoodUserManagement/FastFoodUserManagement.csproj"
COPY . .
WORKDIR "/src/src/Presentation/FastFoodUserManagement"
RUN dotnet build "./FastFoodUserManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FastFoodUserManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastFoodUserManagement.dll"]