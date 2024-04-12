using Tracker.Application.Repositories;
using Tracker.Application.Roles.Queries;
using Tracker.Domain.Users;

namespace Tracker.Application.Roles.Handlers;

public class GetAllRolesQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetAllRolesQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<IReadOnlyCollection<Role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();
        return await tenant.Roles.GetAll(cancellationToken);
    }
}