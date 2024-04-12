using AutoFixture;
using FluentAssertions;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Commands;
using Tracker.Application.WorkflowTemplates.Handlers;
using Tracker.Domain.WorkflowTemplates;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.WorkflowTemplates;

public class UpdateWorkflowTemplateCommandHandlerTests
{
    private readonly IFixture _fixture;

    public UpdateWorkflowTemplateCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task UpdateWorkflowTemplate_TitleAndSteps_Should_CompleteSuccessfully()
    {
        // Arrange
        var stepTemplateExpectedResult = (IReadOnlyCollection<StepTemplate>)_fixture
            .CreateMany<StepTemplate>()
            .ToArray();

        var workflowTemplate = WorkflowTemplate.Create(_fixture.Create<string>(), stepTemplateExpectedResult);
        var workflowTemplateCollection = (IReadOnlyCollection<WorkflowTemplate>)new[] { workflowTemplate };

        var expectedUpdatedTitle = _fixture.Create<string>();
        var expectedUpdatedSteps = (IReadOnlyCollection<StepTemplate>)_fixture.CreateMany<StepTemplate>().ToArray();
        var expectedUpdatedStepIds = (IReadOnlyCollection<Guid>)expectedUpdatedSteps.Select(step => step.Id).ToArray();

        // Workflow template repository mock

        var workflowTemplateRepositoryMock = new Mock<IWorkflowTemplateRepository>(MockBehavior.Strict);
        workflowTemplateRepositoryMock.Setup(workflowTemplateRepository => workflowTemplateRepository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st.Count == 1),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(workflowTemplateCollection));
        workflowTemplateRepositoryMock
            .Setup(repository => repository
                .Update(workflowTemplate, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Step template repository mock

        var stepTemplateRepositoryMock = new Mock<IStepTemplateRepository>();
        stepTemplateRepositoryMock
            .Setup(repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st == expectedUpdatedStepIds),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(expectedUpdatedSteps));

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.WorkflowTemplates)
            .Returns(workflowTemplateRepositoryMock.Object);
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

        var command = new UpdateWorkflowTemplateCommand(
            workflowTemplate.Id,
            expectedUpdatedTitle,
            expectedUpdatedStepIds);
        var sut = new UpdateWorkflowTemplateCommandHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(command, new CancellationToken());

        // Assert
        workflowTemplateRepositoryMock.Verify(repository =>
                repository.Update(workflowTemplate, It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Update was never invoked");

        workflowTemplate.Title.Should().Be(expectedUpdatedTitle);
        workflowTemplate.Steps.Should().BeEquivalentTo(expectedUpdatedSteps);
        Assert.IsType<Guid>(result);
    }
}