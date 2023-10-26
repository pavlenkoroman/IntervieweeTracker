using AutoFixture;
using FluentAssertions;
using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;
using Tracker.Tests.Domain.Extensions;
using Xunit;

namespace Tracker.Tests.Domain.Requests.Workflows;

public class StepTests
{
    private readonly IFixture _fixture;

    public StepTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Step_Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Sample Step";
        var order = 1;
        Guid? userId = Guid.NewGuid();
        Guid? roleId = Guid.NewGuid();
        var comment = "Sample Comment";
        var status = StepStatus.Pending;
        var planningDate = DateTime.Now;

        // Act
        var step = new Step(id, title, order, userId, roleId, comment, status, planningDate);

        // Assert
        step.Id.Should().Be(id);
        step.Title.Should().Be(title);
        step.Order.Should().Be(order);
        step.UserId.Should().Be(userId);
        step.RoleId.Should().Be(roleId);
        step.Comment.Should().Be(comment);
        step.Status.Should().Be(status);
        step.PlanningDate.Should().Be(planningDate);
    }

    [Fact]
    public void CreateByUser_CreatesStepWithUserId()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var title = "Sample Step";
        var order = 1;
        var comment = "Sample Comment";
        var status = StepStatus.Pending;
        var planningDate = DateTime.Now;

        // Act
        var step = Step.CreateByUser(title, order, user.Id, comment, status, planningDate);

        // Assert
        step.UserId.Should().Be(user.Id);
        step.RoleId.Should().BeNull();
    }

    [Fact]
    public void CreateByRole_CreatesStepWithRoleId()
    {
        // Arrange
        var role = _fixture.Create<Role>();
        var title = "Sample Step";
        var order = 1;
        var comment = "Sample Comment";
        var status = StepStatus.Pending;
        var planningDate = DateTime.Now;

        // Act
        var step = Step.CreateByRole(title, order, role.Id, comment, status, planningDate);

        // Assert
        step.UserId.Should().BeNull();
        step.RoleId.Should().Be(role.Id);
    }

    [Fact]
    public void UpdateComment_SetsComment()
    {
        // Arrange
        var step = _fixture.Create<Step>();
        var newComment = "Updated Comment";

        // Act
        step.UpdateComment(newComment);

        // Assert
        step.Comment.Should().Be(newComment);
    }

    [Fact]
    public void Reschedule_SetsPlanningDate()
    {
        // Arrange
        var step = _fixture.Create<Step>();
        var newDate = DateTime.Now.AddDays(1);

        // Act
        step.Reschedule(newDate);

        // Assert
        step.PlanningDate.Should().Be(newDate);
    }

    [Fact]
    public void SetStatus_SetsUserIdAndRoleId()
    {
        // Arrange
        var step = _fixture.Create<Step>();
        var user = _fixture.Create<User>();
        var status = StepStatus.Approved;

        // Act
        step.SetStatus(user, status);

        // Assert
        step.UserId.Should().Be(user.Id);
        step.RoleId.Should().Be(user.RoleId);
        step.Status.Should().Be(status);
    }
}