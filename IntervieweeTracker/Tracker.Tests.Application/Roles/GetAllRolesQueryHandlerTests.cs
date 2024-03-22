using AutoFixture;
using FluentAssertions;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.Roles.Handlers;
using Tracker.Application.Roles.Queries;
using Tracker.Domain.Users;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.Roles;

public class GetAllRolesQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetAllRolesQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task GetAllRolesQueryHandler_Should_ReturnCollectionOfRoles()
    {
        // Arrange
        var roles = _fixture.Create<IReadOnlyCollection<Role>>();
        
        var roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
        roleRepositoryMock.Setup(userRepository => userRepository
                .GetAll(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(roles));

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.Roles)
            .Returns(roleRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var query = new GetAllRolesQuery();
        var sut = new GetAllRolesQueryHandler(tenantRepositoryMock.Object);
        
        // Act
        var result = await sut.Handle(query, new CancellationToken());
        
        // Assert
        roleRepositoryMock.Verify(
            repository => repository
                .GetAll(It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get all method was never invoked");

        Assert.IsAssignableFrom<IReadOnlyCollection<Role>>(result);
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(roles);
    }
}