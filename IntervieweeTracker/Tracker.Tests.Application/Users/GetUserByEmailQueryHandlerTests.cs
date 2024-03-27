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

public class GetUserByEmailQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetUserByEmailQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task GetUserByEmailMustReturnProperUser()
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

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.Users)
            .Returns(userRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var query = new GetUserByEmailQuery(userEmail.Value);
        var sut = new GetUserByEmailQueryHandler(tenantRepositoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        userRepositoryMock.Verify(
            repository => repository.GetByEmail(
                It.Is<Email>(e => e.Value == userEmail.Value),
                It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by email method was never invoked");

        Assert.IsType<User?>(result);
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Name);
        result.RoleId.Should().Be(user.RoleId);
        result.Email.Value.Should().Be(userEmail.Value);
    }
}