using AutoFixture;
using FluentAssertions;
using Tracker.Domain.Requests;
using Tracker.Domain.Requests.Events;
using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;
using Tracker.Tests.Domain.Extensions;
using Xunit;

namespace Tracker.Tests.Domain.Requests;

public class RequestTests
{
    private readonly Fixture _fixture;

    public RequestTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Request_Creation_Succeeds()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var document = _fixture.Create<Document>();
        var workflow = _fixture.Create<Workflow>();

        // Act
        var request = Request.Create(userId, document, workflow);

        // Assert
        request.Id.Should().NotBe(Guid.Empty);
        request.UserId.Should().Be(userId);
        request.Document.Should().Be(document);
        request.Workflow.Should().Be(workflow);
    }

    [Fact]
    public void Create_Request_Succeeds()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var document = _fixture.Create<Document>();
        var workflow = _fixture.Create<Workflow>();

        // Act
        var request = Request.Create(userId, document, workflow);

        // Assert
        request.Id.Should().NotBe(Guid.Empty);
        request.UserId.Should().Be(userId);
        request.Document.Should().Be(document);
        request.Workflow.Should().Be(workflow);
        request.Events.Should().ContainSingle().Which.Should().BeOfType<RequestCreatedEvent>();
    }

    [Fact]
    public void Approve_Request_Succeeds()
    {
        // Arrange
        var request = _fixture.Create<Request>();
        var user = _fixture.Create<User>();
        var approvesCount = request.Workflow.Steps.Count(x => x.Status == StepStatus.Approved);

        // Act
        request.Approve(user);

        // Assert
        request.Workflow.Steps.Count(x => x.Status == StepStatus.Approved).Should().Be(approvesCount + 1);
    }

    [Fact]
    public void Reject_Request_Succeeds()
    {
        // Arrange
        var request = _fixture.Create<Request>();
        var user = _fixture.Create<User>();

        // Act
        request.Reject(user);

        // Assert
        request.Events.Last().Should().BeOfType<RequestRejectedEvent>();
    }

    [Fact]
    public void RestartInterview_Request_Succeeds()
    {
        // Arrange
        var request = _fixture.Create<Request>();
        var user = _fixture.Create<User>();

        // Act
        request.RestartInterview(user);

        // Assert
        request.Workflow.Steps.Count(x => x.Status == StepStatus.Approved).Should().Be(0);
    }

    [Fact]
    public void ChangeInterviewer_Succeeds()
    {
        // Arrange
        var request = _fixture.Create<Request>();
        var newUser = _fixture.Create<User>();

        // Act
        var action = () => request.ChangeInterviewer(newUser);

        // Assert
        action.Should().NotThrow();
        request.UserId.Should().Be(newUser.Id);
    }

    [Fact]
    public void CreateRequest_With_Invalid_UserId_Throws_ArgumentException()
    {
        // Arrange
        var userId = Guid.Empty;
        var document = _fixture.Create<Document>();
        var workflow = _fixture.Create<Workflow>();

        // Act
        Action action = () => Request.Create(userId, document, workflow);
        
        // Assert
        action.Should().Throw<ArgumentException>("Guid cannot be empty (Parameter 'UserId')");
    }
    
    [Fact]
    public void CreateRequest_From_Constructor_With_Invalid_RoleId_Throws_ArgumentException()
    {
        // Arrange
        var userId = Guid.Empty;
        var document = _fixture.Create<Document>();
        var workflow = _fixture.Create<Workflow>();

        // Act
        Action action = () => new Request(Guid.NewGuid(), userId, document, workflow);
        
        // Assert
        action.Should().Throw<ArgumentException>("Guid cannot be empty (Parameter 'UserId')");
    }
    
    [Fact]
    public void CreateRequest_From_Constructor_With_Invalid_Id_Throws_ArgumentException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var document = _fixture.Create<Document>();
        var workflow = _fixture.Create<Workflow>();

        // Act
        Action action = () => new Request(Guid.Empty, userId, document, workflow);
        
        // Assert
        action.Should().Throw<ArgumentException>("Guid cannot be empty (Parameter 'Id')");
    }
}