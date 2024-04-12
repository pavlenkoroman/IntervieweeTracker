using Tracker.Application.Repositories;
using Tracker.Application.Requests.Queries;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Handlers;

public class GetAllRequestsQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetAllRequestsQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<IReadOnlyCollection<Request>> Handle(
        GetAllRequestsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        return await tenant.Requests.GetAll(cancellationToken);
    }
}