using Tracker.Application.Repositories;
using Tracker.Application.Roles.Queries;
using Tracker.Domain.Users;

namespace Tracker.Application.Roles.Handlers;

public class GetRoleByIdQueryHandler
{
    private readonly ITenantRepository _tenant;

    public GetRoleByIdQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Role> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rolesCollection = await _tenant.Roles.GetByIds(new[] { request.RoleId }, cancellationToken);
        return rolesCollection.Single();
    }
}