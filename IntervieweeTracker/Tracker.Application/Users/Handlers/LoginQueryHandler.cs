using Tracker.Application.Repositories;
using Tracker.Application.Users.Queries;
using Tracker.Domain.Users;

namespace Tracker.Application.Users.Handlers;

public class LoginQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public LoginQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Guid> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        var user = await tenant.Users.GetByEmail(new Email(request.Email), cancellationToken);

        if (user is null) throw new ArgumentException("Login failed", nameof(LoginQuery));

        if (user.VerifyPassword(request.Password))
        {
            return user.Id;
        }

        throw new ArgumentException("Login failed", nameof(LoginQuery));
    }
}