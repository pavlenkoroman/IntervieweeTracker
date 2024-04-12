using AutoFixture;
using FluentAssertions;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;
using Tracker.Application.Requests.Handlers;
using Tracker.Domain.Requests;
using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.Requests;

public class RestartRequestCommandHandlerTests
{
    private readonly IFixture _fixture;

    public RestartRequestCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task Restart_Request_Should_ResetAllStepsToPending()
    {
        // Arrange
        var request = _fixture.Create<Request>();
        var user = _fixture.Create<User>();
        request.Approve(user);
        request.Approve(user);

        var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
        requestRepositoryMock
            .Setup(repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(x => x.Count == 1 && x.Contains(request.Id)),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyCollection<Request>>(new[] { request }));

        var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepositoryMock
            .Setup(repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(x => x.Count == 1 && x.Contains(user.Id)),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyCollection<User>>(new[] { user }));

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.Requests)
            .Returns(requestRepositoryMock.Object);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.Users)
            .Returns(userRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        tenantRepositoryMock
            .Setup(tenant => tenant.Dispose()).Verifiable();

        var tenantRepositoryFactoryMock = new Mock<ITenantRepositoryFactory>();
        tenantRepositoryFactoryMock
            .Setup(tenantRepositoryFactory => tenantRepositoryFactory.GetTenant())
            .Returns(tenantRepositoryMock.Object);

        var query = new RestartRequestCommand(request.Id, user.Id);
        var sut = new RestartRequestCommandHandler(tenantRepositoryFactoryMock.Object);

        //Act
        await sut.Handle(query, new CancellationToken());

        // Assert
        requestRepositoryMock.Verify(
            repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(x => x.Count == 1 && x.Contains(request.Id)),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by ids method of request repository was never invoked");
        userRepositoryMock.Verify(
            repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(x => x.Count == 1 && x.Contains(user.Id)),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by ids method of user repository was never invoked");
        tenantRepositoryMock.Verify(
            t => t.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Commit method was never invoked");

        var stepsStatus = request.Workflow.Steps.Select(step => step.Status);
        stepsStatus.Should().OnlyContain(status => status == StepStatus.Pending);
    }
}