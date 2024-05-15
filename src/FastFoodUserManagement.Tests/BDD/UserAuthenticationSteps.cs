using AutoMapper;
using FastFoodUserManagement.Application.UseCases.AuthenticateUser;
using FastFoodUserManagement.Domain.Contracts.Authentication;
using FastFoodUserManagement.Domain.Contracts.Repositories;
using FastFoodUserManagement.Domain.Entities;
using Moq;
using System.Threading;
using TechTalk.SpecFlow;

namespace FastFoodUserManagement.Tests.BDD;

[Binding]
public class UserAuthenticationSteps
{
    private readonly Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
    private readonly Mock<IUserAuthentication> userAuthenticationMock = new Mock<IUserAuthentication>();
    private readonly Mock<IMapper> mapperMock = new Mock<IMapper>();
    private AuthenticateUserRequest request;
    private AuthenticateUserResponse response;
    private string expectedToken = "testToken";
    CancellationToken cancellationToken = new CancellationToken();

    [Given(@"a user with CPF ""(.*)"" exists in the system")]
    public void GivenAUserWithCPFExistsInTheSystem(string cpf)
    {
        var userEntity = new UserEntity { Email = "test@example.com", Identification = cpf };

        userRepositoryMock.Setup(u => u.GetUserByCPFOrEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);

        userAuthenticationMock.Setup(u => u.AuthenticateUser(userEntity, cancellationToken))
            .ReturnsAsync(expectedToken);

        request = new AuthenticateUserRequest(cpf);
    }

    [When(@"the user attempts to authenticate")]
    public async void WhenTheUserAttemptsToAuthenticate()
    {
        var handler = new AuthenticateUserHandler(userRepositoryMock.Object, mapperMock.Object, userAuthenticationMock.Object);
        //var userEntity = await userRepositoryMock.Object.GetUserByCPFOrEmailAsync(request.cpf, string.Empty, cancellationToken);

        var expectedResponse = new AuthenticateUserResponse { Email = "test@example.com", Token = expectedToken };
        mapperMock.Setup(m => m.Map<AuthenticateUserResponse>(It.IsAny<UserEntity>())).Returns(expectedResponse);

        response = await handler.Handle(request, cancellationToken);
    }

    [Then(@"the system should return a token")]
    public void ThenTheSystemShouldReturnAToken()
    {
        Assert.NotNull(response.Token);
    }

    [Then(@"the token should be valid")]
    public void ThenTheTokenShouldBeValid()
    {
        Assert.Equal(response.Token, expectedToken);
    }
}
