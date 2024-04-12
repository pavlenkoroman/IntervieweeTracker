using Tracker.Application.Repositories;
using Tracker.Application.Requests.Queries;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Handlers;

public class GetRequestByIdQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetRequestByIdQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Request> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();
        var requestsCollection = await tenant.Requests.GetByIds(new[] { request.RequestId }, cancellationToken);

        return requestsCollection.Single();
    }
}