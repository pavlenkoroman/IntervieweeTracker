using Tracker.Application.Repositories;
using Tracker.Application.Roles.Commands;
using Tracker.Domain.Users;

namespace Tracker.Application.Roles.Handlers;

public class CreateRoleCommandHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public CreateRoleCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        var role = Role.Create(request.Title);

        await tenant.Roles.Create(role, cancellationToken);

        await tenant.CommitAsync(cancellationToken);

        return role.Id;
    }
}