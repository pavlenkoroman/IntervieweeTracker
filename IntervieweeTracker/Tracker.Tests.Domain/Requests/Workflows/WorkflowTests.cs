using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;
using Tracker.Tests.Domain.Extensions;

namespace Tracker.Tests.Domain.Requests.Workflows;

using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Xunit;

public class WorkflowTests
{
    private readonly Fixture _fixture;

    public WorkflowTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void CreateWorkflow_WithValidParameters_ShouldSucceed()
    {
        // Arrange
        var workflowTemplateId = Guid.NewGuid();
        var title = "Test Workflow";
        var steps = _fixture.CreateMany<Step>().ToList();

        // Act
        var workflow = Workflow.Create(workflowTemplateId, title, steps);

        // Assert
        workflow.Id.Should().NotBe(Guid.Empty);
        workflow.WorkflowTemplateId.Should().Be(workflowTemplateId);
        workflow.Title.Should().Be(title);
        workflow.Steps.Should().BeEquivalentTo(steps);
    }

    [Fact]
    public void ApproveStep_WhenPendingStepExists_ShouldApproveStep()
    {
        // Arrange
        var user = _fixture.Create<User>();

        var stepToApprove = Step.CreateByUser(
            "some title",
            0,
            Guid.NewGuid(),
            "sample",
            StepStatus.Pending,
            DateTime.UtcNow);

        var steps = new List<Step> { stepToApprove };
        var workflow = new Workflow(Guid.NewGuid(), Guid.NewGuid(), "Test Workflow", steps);

        // Act
        workflow.ApproveStep(user);

        // Assert
        stepToApprove.Status.Should().Be(StepStatus.Approved);
    }

    [Fact]
    public void ApproveStep_WhenNoPendingStepExists_ShouldThrowArgumentException()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var steps = new List<Step>
        {
            Step.CreateByUser(
                "some title",
                0,
                Guid.NewGuid(),
                "sample",
                StepStatus.Approved,
                DateTime.UtcNow)
        };

        var workflow = new Workflow(Guid.NewGuid(), Guid.NewGuid(), "Test Workflow", steps);

        // Act
        Action act = () => workflow.ApproveStep(user);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Pending steps not found (Parameter 'stepToApprove')");
    }

    [Fact]
    public void RestartWorkflow_ShouldSetAllStepsToPendingStatus()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var steps = new List<Step>
        {
            Step.CreateByUser(
                "some title",
                0,
                Guid.NewGuid(),
                "sample",
                StepStatus.Approved,
                DateTime.UtcNow),
            Step.CreateByUser(
                "some title",
                0,
                Guid.NewGuid(),
                "sample",
                StepStatus.Pending,
                DateTime.UtcNow),
            Step.CreateByUser(
                "some title",
                0,
                Guid.NewGuid(),
                "sample",
                StepStatus.Rejected,
                DateTime.UtcNow)
        };

        var workflow = Workflow.Create(Guid.NewGuid(), "Test Workflow", steps);

        // Act
        workflow.RestartWorkflow(user);

        // Assert
        foreach (var step in workflow.Steps)
        {
            step.Status.Should().Be(StepStatus.Pending);
        }
    }
    
    [Fact]
    public void ApproveStep_WithInvalidParameters_ShouldSucceed()
    {
        // Arrange
        var workflowTemplateId = Guid.NewGuid();
        var title = "Test Workflow";
        var steps = new List<Step>()
        {
            new(
                Guid.NewGuid(),
                "asd",
                0,
                Guid.NewGuid(),
                Guid.NewGuid(),
                "comm",
                StepStatus.Approved,
                DateTime.UtcNow)
        };

        // Act
        Action act = () => Workflow.Create(workflowTemplateId, title, steps)
            .ApproveStep(
                new User(
                    Guid.NewGuid(), 
                    Guid.NewGuid(), 
                    "asd", 
                    _fixture.Create<Email>(),
                    _fixture.Create<string>()));

        // Assert
        act.Should().Throw<ArgumentException>("Pending steps not found (Parameter: stepsToApprove)");
    }
    
    [Fact]
    public void RejectStep_WithInvalidParameters_ShouldSucceed()
    {
        // Arrange
        var workflowTemplateId = Guid.NewGuid();
        var title = "Test Workflow";
        var steps = new List<Step>()
        {
            new(
                Guid.NewGuid(),
                "asd",
                0,
                Guid.NewGuid(),
                Guid.NewGuid(),
                "comm",
                StepStatus.Approved,
                DateTime.UtcNow)
        };

        // Act
        Action act = () => Workflow.Create(workflowTemplateId, title, steps)
            .RejectStep(
                new User(
                    Guid.NewGuid(), 
                    Guid.NewGuid(), 
                    "asd", 
                    _fixture.Create<Email>(),
                    _fixture.Create<string>()));

        // Assert
        act.Should().Throw<ArgumentException>("Pending steps not found (Parameter: stepsToReject)");
    }
}