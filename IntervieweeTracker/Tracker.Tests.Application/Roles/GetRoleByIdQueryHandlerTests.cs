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

public class GetRoleByIdQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetRoleByIdQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task GetRoleById_Should_ReturnRole()
    {
        // Arrange
        var roleTitle = _fixture.Create<string>();
        var role = Role.Create(roleTitle);
        var rolesExpectedResult = (IReadOnlyCollection<Role>)new[] { role };

        var roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
        roleRepositoryMock
            .Setup(repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(r => r.Count == 1 && r.Contains(role.Id)),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(rolesExpectedResult));


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

        var query = new GetRoleByIdQuery(role.Id);
        var sut = new GetRoleByIdQueryHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        roleRepositoryMock.Verify(
            repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(r => r.Count == 1 && r.Contains(role.Id)),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by ids method was never invoked");

        Assert.IsType<Role?>(result);
        result.Id.Should().Be(role.Id);
        result.Title.Should().Be(role.Title);
    }
}