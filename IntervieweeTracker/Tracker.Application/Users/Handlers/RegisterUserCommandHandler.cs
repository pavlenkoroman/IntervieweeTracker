using Tracker.Application.Repositories;
using Tracker.Application.Users.Commands;
using Tracker.Domain.Users;

namespace Tracker.Application.Users.Handlers;

public class RegisterUserCommandHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public RegisterUserCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();
        
        var user = User.Create(request.RoleId, request.Name, new Email(request.Email), request.Password);
        await tenant.Users.Create(user, cancellationToken);

        await tenant.CommitAsync(cancellationToken);

        return user.Id;
    }
}