using Tracker.Application.Repositories;
using Tracker.Application.Roles.Queries;
using Tracker.Domain.Users;

namespace Tracker.Application.Roles.Handlers;

public class GetRoleByIdQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetRoleByIdQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Role> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();
        var rolesCollection = await tenant.Roles.GetByIds(new[] { request.RoleId }, cancellationToken);
        return rolesCollection.Single();
    }
}