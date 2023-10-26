using Tracker.Domain.Users;
using Tracker.Domain.WorkflowTemplates;
using Tracker.Tests.Domain.Extensions;

namespace Tracker.Tests.Domain.WorkflowTemplates;

using System;
using AutoFixture;
using FluentAssertions;
using Xunit;

public class StepTemplateTests
{
    private readonly IFixture _fixture;

    public StepTemplateTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void StepTemplate_CreateByRole_WithValidArguments_ShouldInitializeProperties()
    {
        // Arrange
        var title = "Test Title";
        var order = 1;
        Guid? roleId = Guid.NewGuid();

        // Act
        var stepTemplate = StepTemplate.CreateByRole(title, order, roleId);

        // Assert
        stepTemplate.Id.Should().NotBeEmpty();
        stepTemplate.Title.Should().NotBeEmpty();
        stepTemplate.Order.Should().BePositive();
        stepTemplate.UserId.Should().BeNull();
        stepTemplate.RoleId.Should().NotBeEmpty();
    }

    [Fact]
    public void StepTemplate_CreateByUser_WithValidArguments_ShouldInitializeProperties()
    {
        // Arrange
        var title = "Test Title";
        var order = 1;
        Guid? userId = Guid.NewGuid();

        // Act
        var stepTemplate = StepTemplate.CreateByUser(title, order, userId);

        // Assert
        stepTemplate.Id.Should().NotBeEmpty();
        stepTemplate.Title.Should().NotBeEmpty();
        stepTemplate.Order.Should().BePositive();
        stepTemplate.UserId.Should().NotBeNull();
        stepTemplate.UserId.Should().NotBeEmpty();
        stepTemplate.RoleId.Should().BeNull();
    }

    [Fact]
    public void StepTemplate_Constructor_WithInvalidTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        string? title = null;
        var order = 1;
        Guid? userId = Guid.NewGuid();
        Guid? roleId = Guid.NewGuid();

        // Act & Assert
        Action act = () => new StepTemplate(id, title, order, userId, roleId);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Title cannot be null, empty, or whitespace (Parameter 'title')");
    }

    [Fact]
    public void StepTemplate_Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var title = "Test Title";
        var order = 1;
        Guid? userId = Guid.NewGuid();
        Guid? roleId = Guid.NewGuid();

        // Act & Assert
        Action act = () => new StepTemplate(id, title, order, userId, roleId);
        act.Should().Throw<ArgumentException>().WithMessage("Guid cannot be empty (Parameter 'id')");
    }

    [Fact]
    public void StepTemplate_UpdateUser_ShouldUpdateUserIdAndRoleId()
    {
        // Arrange
        var stepTemplate = _fixture.Create<StepTemplate>();
        var user = _fixture.Create<User>();

        // Act
        stepTemplate.UpdateUser(user);

        // Assert
        stepTemplate.UserId.Should().Be(user.Id);
        stepTemplate.RoleId.Should().Be(user.RoleId);
    }
}