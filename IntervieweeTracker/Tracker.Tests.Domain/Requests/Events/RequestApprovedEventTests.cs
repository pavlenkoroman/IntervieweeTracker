using AutoFixture;
using FluentAssertions;
using Tracker.Domain.Requests.Events;
using Tracker.Tests.Domain.Extensions;
using Xunit;

namespace Tracker.Tests.Domain.Requests.Events;

public class RequestApprovedEventTests
{
    private readonly IFixture _fixture;

    public RequestApprovedEventTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Create_RequestApprovedEvent_Succeeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        var requestApprovedEvent = new RequestApprovedEvent(id, requestId, data);

        // Assert
        requestApprovedEvent.Id.Should().NotBeEmpty();
        requestApprovedEvent.RequestId.Should().NotBeEmpty();
        requestApprovedEvent.Data.Should().Be(data);
    }

    [Fact]
    public void Create_RequestApprovedEvent_WithEmptyId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        Action action = () => new RequestApprovedEvent(id, requestId, data);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Guid cannot be empty (Parameter 'id')");
    }

    [Fact]
    public void Create_RequestApprovedEvent_WithEmptyRequestId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.Empty;
        var data = "Approval data";

        // Act
        Action action = () => new RequestApprovedEvent(id, requestId, data);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("RequestId cannot be empty (Parameter 'requestId')");
    }

    [Fact]
    public void Create_RequestApprovedEvent_WithEmptyData_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = string.Empty;

        // Act
        Action action = () => new RequestApprovedEvent(id, requestId, data);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>();
    }
}