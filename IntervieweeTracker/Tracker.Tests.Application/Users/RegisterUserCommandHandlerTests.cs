using AutoFixture;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.Users.Commands;
using Tracker.Application.Users.Handlers;
using Tracker.Domain.Users;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.Users;

public class RegisterUserCommandHandlerTests
{
    private readonly IFixture _fixture;

    public RegisterUserCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task RegisterUserCommandHandler_Should_Return_TypeofGuidUserId()
    {
        // Arrange
        var userRoleId = Guid.NewGuid();
        var userName = _fixture.Create<string>();
        var userEmail = _fixture.Create<Email>();
        var password = _fixture.Create<string>();
        var user = User.Create(userRoleId, userName, userEmail, password);

        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepositoryMock
            .Setup(repository => repository.Create(
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

        var command = new RegisterUserCommand(user.RoleId, user.Name, user.Email.Value, password);
        var sut = new RegisterUserCommandHandler(tenantRepositoryMock.Object);

        // Act
        var result = await sut.Handle(command, new CancellationToken());

        // Assert
        userRepositoryMock.Verify(
            repository => repository.Create(
                It.Is<User>(u => u.RoleId == userRoleId && u.Name == userName && u.Email == userEmail),
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