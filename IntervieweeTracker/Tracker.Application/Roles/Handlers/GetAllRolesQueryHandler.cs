using Tracker.Application.Repositories;
using Tracker.Application.Roles.Queries;
using Tracker.Domain.Users;

namespace Tracker.Application.Roles.Handlers;

public class GetAllRolesQueryHandler
{
    private readonly ITenantRepository _tenant;

    public GetAllRolesQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<IReadOnlyCollection<Role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _tenant.Roles.GetAll(cancellationToken);
    }
}