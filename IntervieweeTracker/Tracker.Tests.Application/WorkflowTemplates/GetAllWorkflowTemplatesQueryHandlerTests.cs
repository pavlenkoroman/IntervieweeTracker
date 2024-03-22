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

public class GetAllWorkflowTemplatesQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetAllWorkflowTemplatesQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task GetAllWorkflowTemplates_ShouldReturn_Workflows()
    {
        // Arrange
        var workflowTemplates = (IReadOnlyCollection<WorkflowTemplate>)_fixture
            .CreateMany<WorkflowTemplate>()
            .ToArray();

        var workflowTemplateRepositoryMock = new Mock<IWorkflowTemplateRepository>(MockBehavior.Strict);
        workflowTemplateRepositoryMock.Setup(workflowTemplateRepository => workflowTemplateRepository
                .GetAll(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(workflowTemplates));

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.WorkflowTemplates)
            .Returns(workflowTemplateRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var query = new GetAllWorkflowTemplatesQuery();
        var sut = new GetAllWorkflowTemplatesQueryHandler(tenantRepositoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        workflowTemplateRepositoryMock.Verify(repository =>
                repository.GetAll(It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get all was never invoked");

        Assert.IsAssignableFrom<IReadOnlyCollection<WorkflowTemplate>>(result);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(workflowTemplates);
    }
}