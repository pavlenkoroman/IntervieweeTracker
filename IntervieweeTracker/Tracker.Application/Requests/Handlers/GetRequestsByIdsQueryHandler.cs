using Tracker.Application.Repositories;
using Tracker.Application.Requests.Queries;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Handlers;

public class GetRequestsByIdsQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetRequestsByIdsQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<IReadOnlyCollection<Request>> Handle(
        GetRequestsByIdsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        return await tenant.Requests.GetByIds(request.RequestIds, cancellationToken);
    }
}