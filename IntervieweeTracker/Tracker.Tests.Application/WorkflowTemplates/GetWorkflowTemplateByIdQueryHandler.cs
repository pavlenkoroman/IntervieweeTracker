using AutoFixture;
using FluentAssertions;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Handlers;
using Tracker.Application.WorkflowTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.WorkflowTemplates;

public class GetWorkflowTemplateByIdQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetWorkflowTemplateByIdQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task GetWorkflowTemplateById_ShouldReturn_Workflow()
    {
        // Arrange
        var stepTemplates = (IReadOnlyCollection<StepTemplate>)_fixture
            .CreateMany<StepTemplate>()
            .ToArray();

        var workflowTemplate = WorkflowTemplate.Create(_fixture.Create<string>(), stepTemplates);
        var workflowTemplateCollection = (IReadOnlyCollection<WorkflowTemplate>)new[] { workflowTemplate };

        var workflowTemplateRepositoryMock = new Mock<IWorkflowTemplateRepository>(MockBehavior.Strict);
        workflowTemplateRepositoryMock.Setup(workflowTemplateRepository => workflowTemplateRepository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st.Count == 1),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(workflowTemplateCollection));

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.WorkflowTemplates)
            .Returns(workflowTemplateRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        tenantRepositoryMock
            .Setup(tenant => tenant.Dispose()).Verifiable();

        var tenantRepositoryFactoryMock = new Mock<ITenantRepositoryFactory>();
        tenantRepositoryFactoryMock
            .Setup(tenantRepositoryFactory => tenantRepositoryFactory.GetTenant())
            .Returns(tenantRepositoryMock.Object);

        var query = new GetWorkflowTemplateByIdQuery(workflowTemplate.Id);
        var sut = new GetWorkflowTemplateByIdQueryHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        workflowTemplateRepositoryMock.Verify(repository =>
                repository.GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st.Count == 1),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by id was never invoked");

        Assert.IsType<WorkflowTemplate>(result);
        result.Should().NotBeNull();
        result.Should().Be(workflowTemplate);
    }
}