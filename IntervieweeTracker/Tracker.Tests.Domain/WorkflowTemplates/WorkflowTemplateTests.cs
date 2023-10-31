using AutoFixture;
using FluentAssertions;
using Tracker.Domain.Requests;
using Tracker.Domain.Users;
using Tracker.Domain.WorkflowTemplates;
using Tracker.Tests.Domain.Extensions;
using Xunit;

namespace Tracker.Tests.Domain.WorkflowTemplates;

public class WorkflowTemplateTests
{
    private readonly IFixture _fixture;

    public WorkflowTemplateTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void WorkflowTemplate_Constructor_WithValidArguments_ShouldInitializeProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Sample Workflow";
        var steps = _fixture.CreateMany<StepTemplate>();

        // Act
        var workflowTemplate = new WorkflowTemplate(id, title, steps.ToList());

        // Assert
        workflowTemplate.Id.Should().Be(id);
        workflowTemplate.Title.Should().Be(title);
        workflowTemplate.Steps.Should().BeEquivalentTo(steps);
    }

    [Fact]
    public void WorkflowTemplate_Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange
        var title = "Sample Workflow";
        var steps = _fixture.CreateMany<StepTemplate>();

        // Act & Assert
        Action act = () => new WorkflowTemplate(Guid.Empty, title, steps.ToList());
        act.Should().Throw<ArgumentException>().WithMessage("Guid cannot be empty (Parameter 'id')");
    }

    [Fact]
    public void WorkflowTemplate_Constructor_WithNoSteps_ShouldThrowArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Sample Workflow";
        var steps = new List<StepTemplate>(); // Empty steps list

        // Act & Assert
        Action act = () => new WorkflowTemplate(id, title, steps);
        act
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Workflow template must contain at least one step (Parameter 'steps')");
    }

    [Fact]
    public void WorkflowTemplate_CreateRequest_ShouldCreateRequestWithCorrectParameters()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var document = _fixture.Create<Document>();
        var id = Guid.NewGuid();
        var title = "Sample Workflow";
        var steps = _fixture.CreateMany<StepTemplate>();
        var workflowTemplate = new WorkflowTemplate(id, title, steps.ToList());

        // Act
        var request = workflowTemplate.CreateRequest(user, document);

        // Assert
        request.UserId.Should().Be(user.Id);
        request.Document.Should().Be(document);
        request.Workflow.Title.Should().Be(title);
        request.Workflow.Steps.Should().HaveSameCount(steps);
    }

    [Fact]
    public void WorkflowTemplate_CreateMethod_WithValid_Data()
    {
        // Arrange
        var title = "Sample Workflow";
        var steps = _fixture.CreateMany<StepTemplate>();

        var workflowTemplate = WorkflowTemplate.Create(title, steps.ToArray());
        
        // Assert
        workflowTemplate.Title.Should().Be(title);
        workflowTemplate.Steps.Should().BeEquivalentTo(steps);
    }
}