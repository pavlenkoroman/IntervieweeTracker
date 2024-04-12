using AutoFixture;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Commands;
using Tracker.Application.WorkflowTemplates.Handlers;
using Tracker.Domain.WorkflowTemplates;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.WorkflowTemplates;

public class CreateWorkflowTemplateCommandHandlerTests
{
    private IFixture _fixture;

    public CreateWorkflowTemplateCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task CreateStepTemplateByUserId_Should_CompleteSuccessfully()
    {
        // Arrange
        var stepTemplateExpectedResult = (IReadOnlyCollection<StepTemplate>)_fixture
            .CreateMany<StepTemplate>()
            .ToArray();
        var stepTemplateExpectedResultIds = (IReadOnlyCollection<Guid>)stepTemplateExpectedResult
            .Select(x => x.Id)
            .ToArray();

        var workflowTemplate = WorkflowTemplate.Create(_fixture.Create<string>(), stepTemplateExpectedResult);

        var workflowTemplateRepositoryMock = new Mock<IWorkflowTemplateRepository>(MockBehavior.Strict);
        workflowTemplateRepositoryMock.Setup(workflowTemplateRepository => workflowTemplateRepository
                .Create(
                    It.Is<WorkflowTemplate>(st =>
                        st.Title == workflowTemplate.Title &&
                        st.Steps == workflowTemplate.Steps),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var stepTemplateRepositoryMock = new Mock<IStepTemplateRepository>();
        stepTemplateRepositoryMock
            .Setup(repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st == stepTemplateExpectedResultIds),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(stepTemplateExpectedResult));

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

        var command = new CreateWorkflowTemplateCommand(workflowTemplate.Title, stepTemplateExpectedResultIds);
        var sut = new CreateWorkflowTemplateCommandHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(command, new CancellationToken());

        // Assert
        workflowTemplateRepositoryMock.Verify(
            repository => repository
                .Create(
                    It.Is<WorkflowTemplate>(st =>
                        st.Title == workflowTemplate.Title &&
                        st.Steps == workflowTemplate.Steps),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Create was never invoked");

        Assert.IsType<Guid>(result);
    }
}