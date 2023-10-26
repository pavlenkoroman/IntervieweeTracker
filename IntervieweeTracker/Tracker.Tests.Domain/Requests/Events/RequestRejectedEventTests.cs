using AutoFixture;
using FluentAssertions;
using Tracker.Domain.Requests.Events;
using Tracker.Tests.Domain.Extensions;
using Xunit;

namespace Tracker.Tests.Domain.Requests.Events;

public class RequestRejectedEventTests
{
    private readonly IFixture _fixture;

    public RequestRejectedEventTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Create_RequestRejectedEvent_Succeeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        var requestApprovedEvent = new RequestRejectedEvent(id, requestId, data);

        // Assert
        requestApprovedEvent.Id.Should().NotBeEmpty();
        requestApprovedEvent.RequestId.Should().NotBeEmpty();
        requestApprovedEvent.Data.Should().Be(data);
    }

    [Fact]
    public void Create_RequestRejectedEvent_WithEmptyId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var requestId = Guid.NewGuid();
        var data = "Approval data";

        // Act
        Action action = () => new RequestRejectedEvent(id, requestId, data);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Guid cannot be empty (Parameter 'id')");
    }

    [Fact]
    public void Create_RequestRejectedEvent_WithEmptyRequestId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.Empty;
        var data = "Approval data";

        // Act
        Action action = () => new RequestRejectedEvent(id, requestId, data);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("RequestId cannot be empty (Parameter 'requestId')");
    }

    [Fact]
    public void Create_RequestRejectedEvent_WithEmptyData_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var data = string.Empty;

        // Act
        Action action = () => new RequestRejectedEvent(id, requestId, data);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>();
    }
}