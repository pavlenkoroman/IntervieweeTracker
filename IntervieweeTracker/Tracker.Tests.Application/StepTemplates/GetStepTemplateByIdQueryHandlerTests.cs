using AutoFixture;
using FluentAssertions;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.Roles.Handlers;
using Tracker.Application.Roles.Queries;
using Tracker.Application.StepTemplates.Handlers;
using Tracker.Application.StepTemplates.Queries;
using Tracker.Domain.Users;
using Tracker.Domain.WorkflowTemplates;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.StepTemplates;

public class GetStepTemplateByIdQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetStepTemplateByIdQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task GetRoleById_Should_ReturnRole()
    {
        // Arrange
        var stepTemplate = StepTemplate.CreateByUser(
            _fixture.Create<string>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>());

        var stepTemplateExpectedResult = (IReadOnlyCollection<StepTemplate>)new[] { stepTemplate };

        var roleRepositoryMock = new Mock<IStepTemplateRepository>(MockBehavior.Strict);
        roleRepositoryMock
            .Setup(repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st.Count == 1 && st.Contains(stepTemplate.Id)),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(stepTemplateExpectedResult));


        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.StepTemplates)
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

        var query = new GetStepByIdQuery(stepTemplate.Id);
        var sut = new GetStepTemplateByIdQueryHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        roleRepositoryMock.Verify(
            repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(r => r.Count == 1 && r.Contains(stepTemplate.Id)),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by ids method was never invoked");

        Assert.IsType<StepTemplate>(result);
        result.Id.Should().Be(stepTemplate.Id);
        result.Title.Should().Be(stepTemplate.Title);
    }
}