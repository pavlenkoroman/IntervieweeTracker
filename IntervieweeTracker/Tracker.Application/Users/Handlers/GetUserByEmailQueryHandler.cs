using Tracker.Application.Repositories;
using Tracker.Application.Users.Queries;
using Tracker.Domain.Users;

namespace Tracker.Application.Users.Handlers;

public class GetUserByEmailQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetUserByEmailQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<User?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();
        return await tenant.Users.GetByEmail(new Email(request.Email), cancellationToken);
    }
}