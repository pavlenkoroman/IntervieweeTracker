using AutoFixture;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;
using Tracker.Application.Requests.Handlers;
using Tracker.Domain.Requests;
using Tracker.Domain.Users;
using Tracker.Domain.WorkflowTemplates;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.Requests;

public class CreateRequestCommandHandlerTests
{
    private readonly IFixture _fixture;

    public CreateRequestCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task CreateRequestCommandHandler_Should_Return_RequestId()
    {
        // Arrange
        var workflowTemplatesCollection = _fixture.CreateMany<WorkflowTemplate>(1).ToArray();
        var usersCollection = _fixture.CreateMany<User>(1).ToArray();
        var document = _fixture.Create<Document>();
        var request = workflowTemplatesCollection.Single().CreateRequest(usersCollection.Single(), document);

        var workflowTemplateId = workflowTemplatesCollection.Single().Id;
        var userId = usersCollection.Single().Id;

        var workflowTemplateRepositoryMock = new Mock<IWorkflowTemplateRepository>(MockBehavior.Strict);
        workflowTemplateRepositoryMock.Setup(repository => repository
                .GetByIds(It.Is<IReadOnlyCollection<Guid>>(x => x.Count == 1), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyCollection<WorkflowTemplate>>(workflowTemplatesCollection));

        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepositoryMock.Setup(repository => repository
                .GetByIds(It.Is<IReadOnlyCollection<Guid>>(x => x.Count == 1), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyCollection<User>>(usersCollection));

        var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
        requestRepositoryMock
            .Setup(repository => repository.Create(
                It.Is<Request>(req =>
                    req.Document == request.Document &&
                    req.UserId == request.UserId &&
                    req.Workflow.WorkflowTemplateId == request.Workflow.WorkflowTemplateId),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(repository => repository.WorkflowTemplates)
            .Returns(workflowTemplateRepositoryMock.Object);
        tenantRepositoryMock
            .SetupGet(repository => repository.Users)
            .Returns(userRepositoryMock.Object);
        tenantRepositoryMock
            .SetupGet(repository => repository.Requests)
            .Returns(requestRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(repository => repository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        tenantRepositoryMock
            .Setup(tenant => tenant.Dispose()).Verifiable();

        var tenantRepositoryFactoryMock = new Mock<ITenantRepositoryFactory>();
        tenantRepositoryFactoryMock
            .Setup(tenantRepositoryFactory => tenantRepositoryFactory.GetTenant())
            .Returns(tenantRepositoryMock.Object);

        var command = new CreateRequestCommand(workflowTemplateId, userId, document);
        var sut = new CreateRequestCommandHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(command, new CancellationToken());

        // Assert
        requestRepositoryMock
            .Verify(repository => repository.Create(
                    It.Is<Request>(req =>
                        req.Document == request.Document &&
                        req.UserId == request.UserId &&
                        req.Workflow.WorkflowTemplateId == request.Workflow.WorkflowTemplateId),
                    It.IsAny<CancellationToken>()),
                Times.Exactly(1),
                "Create request was never invoked");
        tenantRepositoryMock.Verify(
            t => t.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Commit method was never invoked");

        Assert.IsType<Guid>(result);
    }
}