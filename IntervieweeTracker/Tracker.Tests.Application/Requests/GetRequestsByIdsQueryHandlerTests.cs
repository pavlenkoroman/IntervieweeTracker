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

public class GetRequestsByIdsQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetRequestsByIdsQueryHandlerTests()
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
        var requestsIds = (IReadOnlyCollection<Guid>)requests.Select(x => x.Id).ToArray();

        var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
        requestRepositoryMock
            .Setup(repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st == requestsIds),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(requests));

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.Requests)
            .Returns(requestRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var query = new GetRequestsByIdsQuery(requestsIds);
        var sut = new GetRequestsByIdsQueryHandler(tenantRepositoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        requestRepositoryMock.Verify(
            repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(st => st == requestsIds),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by ids method was never invoked");

        Assert.IsAssignableFrom<IReadOnlyCollection<Request>>(result);
        result.Count.Should().Be(requests.Count);
    }
}