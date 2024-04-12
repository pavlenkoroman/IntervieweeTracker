using AutoFixture;
using FluentAssertions;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.StepTemplates.Handlers;
using Tracker.Application.StepTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.StepTemplates;

public class GetStepTemplatesByIdsQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetStepTemplatesByIdsQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task GetStepTemplatesByIdsQueryHandler_Should_ReturnStepTemplatesCollection()
    {
        // Arrange
        var stepTemplateExpectedResult = (IReadOnlyCollection<StepTemplate>)_fixture
            .CreateMany<StepTemplate>()
            .ToArray();
        var stepTemplateExpectedResultIds = (IReadOnlyCollection<Guid>)stepTemplateExpectedResult
            .Select(x => x.Id)
            .ToArray();

        var stepTemplateRepositoryMock = new Mock<IStepTemplateRepository>(MockBehavior.Strict);
        stepTemplateRepositoryMock
            .Setup(repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st == stepTemplateExpectedResultIds),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(stepTemplateExpectedResult));

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.StepTemplates)
            .Returns(stepTemplateRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        tenantRepositoryMock
            .Setup(tenant => tenant.Dispose()).Verifiable();
        
        var tenantRepositoryFactoryMock = new Mock<ITenantRepositoryFactory>();
        tenantRepositoryFactoryMock
            .Setup(tenantRepositoryFactory => tenantRepositoryFactory.GetTenant())
            .Returns(tenantRepositoryMock.Object);

        var query = new GetStepsByIdsQuery(stepTemplateExpectedResultIds);
        var sut = new GetStepTemplatesByIdsQueryHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        stepTemplateRepositoryMock.Verify(
            repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st == stepTemplateExpectedResultIds),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by ids method was never invoked");

        Assert.IsAssignableFrom<IReadOnlyCollection<StepTemplate>>(result);
        result.Count.Should().Be(stepTemplateExpectedResult.Count);
    }
}