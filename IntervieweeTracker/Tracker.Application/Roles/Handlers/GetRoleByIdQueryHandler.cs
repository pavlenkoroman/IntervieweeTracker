using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Roles.Queries;
using Tracker.Domain.Users;

namespace Tracker.Application.Roles.Handlers;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Role>
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

        return await _tenant.Roles.GetById(request.RoleId, cancellationToken);
    }
}