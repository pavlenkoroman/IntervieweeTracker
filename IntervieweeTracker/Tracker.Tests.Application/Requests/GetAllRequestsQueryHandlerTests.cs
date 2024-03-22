using AutoFixture;
using FluentAssertions;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Handlers;
using Tracker.Application.Requests.Queries;
using Tracker.Domain.Requests;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.Requests;

public class GetAllRequestsQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetAllRequestsQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task GetRequestsByIdsQueryHandler_Should_Return_RequestCollection()
    {
        // Arrange
        var requests = (IReadOnlyCollection<Request>)_fixture
            .CreateMany<Request>()
            .ToArray();

        var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
        requestRepositoryMock
            .Setup(repository => repository.GetAll(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(requests));

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.Requests)
            .Returns(requestRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var query = new GetAllRequestsQuery();
        var sut = new GetAllRequestsQueryHandler(tenantRepositoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        requestRepositoryMock.Verify(
            repository => repository.GetAll(It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by ids method was never invoked");

        Assert.IsAssignableFrom<IReadOnlyCollection<Request>>(result);
        result.Count.Should().Be(requests.Count);
    }
}