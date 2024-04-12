using AutoFixture;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.Roles.Commands;
using Tracker.Application.Roles.Handlers;
using Tracker.Domain.Users;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.Roles;

public class CreateRoleCommandHandlerTests
{
    private readonly IFixture _fixture;

    public CreateRoleCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task RegisterUserCommandHandler_Should_Return_TypeofGuidUserId()
    {
        // Arrange
        var roleTitle = _fixture.Create<string>();
        var role = Role.Create(roleTitle);

        var roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
        roleRepositoryMock
            .Setup(repository => repository
                .Create(
                    It.Is<Role>(r => r.Title == roleTitle),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.Roles)
            .Returns(roleRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        tenantRepositoryMock
            .Setup(tenant => tenant.Dispose()).Verifiable();
        
        var tenantRepositoryFactoryMock = new Mock<ITenantRepositoryFactory>();
        tenantRepositoryFactoryMock
            .Setup(tenantRepositoryFactory => tenantRepositoryFactory.GetTenant())
            .Returns(tenantRepositoryMock.Object);

        var command = new CreateRoleCommand(role.Title);
        var sut = new CreateRoleCommandHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(command, new CancellationToken());

        // Assert
        roleRepositoryMock.Verify(
            repository => repository
                .Create(
                    It.Is<Role>(r => r.Title == roleTitle),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Create method was never invoked");
        tenantRepositoryMock.Verify(
            repository => repository.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Commit method was never invoked");

        Assert.IsType<Guid>(result);
    }
}