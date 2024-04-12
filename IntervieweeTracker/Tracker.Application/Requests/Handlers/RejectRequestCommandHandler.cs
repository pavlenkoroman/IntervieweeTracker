using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class RejectRequestCommandHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public RejectRequestCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task Handle(RejectRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        var interviewRequestCollection = await tenant.Requests
            .GetByIds(new[] { request.RequestId }, cancellationToken)
            .ConfigureAwait(false);

        var userCollection = await tenant.Users.GetByIds(new[] { request.UserId }, cancellationToken);

        interviewRequestCollection.Single().Reject(userCollection.Single());
        await tenant.CommitAsync(cancellationToken);
    }
}