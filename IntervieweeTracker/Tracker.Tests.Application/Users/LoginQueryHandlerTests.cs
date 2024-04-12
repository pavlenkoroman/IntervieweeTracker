using AutoFixture;
using FluentAssertions;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.Users.Handlers;
using Tracker.Application.Users.Queries;
using Tracker.Domain.Users;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.Users;

public class LoginQueryHandlerTests
{
    private readonly IFixture _fixture;

    public LoginQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task LoginMustReturnProperUserId()
    {
        // Arrange
        var userRoleId = Guid.NewGuid();
        var userName = _fixture.Create<string>();
        var userEmail = _fixture.Create<Email>();
        var password = _fixture.Create<string>();
        var user = User.Create(userRoleId, userName, userEmail, password);

        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepositoryMock
            .Setup(repository => repository.GetByEmail(
                It.Is<Email>(e => e.Value == userEmail.Value),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(user)!);
        userRepositoryMock
            .Setup(repository => repository.Login(
                It.Is<User>(u => u.Email == userEmail && u.Name == userName && u.RoleId == userRoleId),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.Users)
            .Returns(userRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        tenantRepositoryMock
            .Setup(tenant => tenant.Dispose()).Verifiable();

        var tenantRepositoryFactoryMock = new Mock<ITenantRepositoryFactory>();
        tenantRepositoryFactoryMock
            .Setup(tenantRepositoryFactory => tenantRepositoryFactory.GetTenant())
            .Returns(tenantRepositoryMock.Object);

        var query = new LoginQuery(userEmail.Value);
        var sut = new LoginQueryHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        userRepositoryMock.Verify(
            repository => repository
                .GetByEmail(It.Is<Email>(e => e.Value == userEmail.Value), It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by email method was never invoked");

        Assert.IsType<Guid>(result);
    }

    [Fact]
    public async Task LoginWithInvalidUserShouldThrowException()
    {
        // Arrange
        var userRoleId = Guid.NewGuid();
        var userName = _fixture.Create<string>();
        var userEmail = _fixture.Create<Email>();

        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Loose);
        userRepositoryMock
            .Setup(repository => repository.GetByEmail(
                It.Is<Email>(e => e.Value != userEmail.Value),
                It.IsAny<CancellationToken>()))
            .Returns(value: null);
        userRepositoryMock
            .Setup(repository => repository.Login(
                It.Is<User>(u => u.Email == userEmail && u.Name == userName && u.RoleId == userRoleId),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(repository => repository.Users)
            .Returns(userRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(repository => repository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        tenantRepositoryMock
            .Setup(tenant => tenant.Dispose()).Verifiable();

        var tenantRepositoryFactoryMock = new Mock<ITenantRepositoryFactory>();
        tenantRepositoryFactoryMock
            .Setup(tenantRepositoryFactory => tenantRepositoryFactory.GetTenant())
            .Returns(tenantRepositoryMock.Object);

        var query = new LoginQuery(userEmail.Value);
        var sut = new LoginQueryHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var action = async () => await sut.Handle(query, new CancellationToken());

        // Assert
        await action.Should().ThrowAsync<ArgumentException>("Current user does not exist");
    }
}