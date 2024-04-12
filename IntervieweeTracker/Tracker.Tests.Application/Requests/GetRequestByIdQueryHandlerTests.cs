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

public class GetRequestByIdQueryHandlerTests
{
    private readonly IFixture _fixture;

    public GetRequestByIdQueryHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task GetRequestByIdQueryHandler_Should_Return_Request()
    {
        // Arrange
        var request = _fixture.Create<Request>();

        var requestRepositoryMock = new Mock<IRequestRepository>(MockBehavior.Strict);
        requestRepositoryMock
            .Setup(repository => repository.GetByIds(
                It.Is<IReadOnlyCollection<Guid>>(x => x.Count == 1 && x.Contains(request.Id)),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IReadOnlyCollection<Request>>(new[] { request }));

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock.SetupGet(repository => repository.Requests).Returns(requestRepositoryMock.Object);
        tenantRepositoryMock
            .Setup(repository => repository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        tenantRepositoryMock
            .Setup(tenant => tenant.Dispose()).Verifiable();

        var tenantRepositoryFactoryMock = new Mock<ITenantRepositoryFactory>();
        tenantRepositoryFactoryMock
            .Setup(tenantRepositoryFactory => tenantRepositoryFactory.GetTenant())
            .Returns(tenantRepositoryMock.Object);

        var query = new GetRequestByIdQuery(request.Id);
        var sut = new GetRequestByIdQueryHandler(tenantRepositoryFactoryMock.Object);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        requestRepositoryMock.Verify(
            repository => repository
                .GetByIds(
                    It.Is<IReadOnlyCollection<Guid>>(r => r.Count == 1 && r.Contains(request.Id)),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Get by ids method was never invoked");

        Assert.IsType<Request>(result);
        result.Id.Should().Be(request.Id);
    }
}