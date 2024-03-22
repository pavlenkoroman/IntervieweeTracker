using Tracker.Application.Repositories;
using Tracker.Application.Users.Commands;
using Tracker.Domain.Users;

namespace Tracker.Application.Users.Handlers;

public class RegisterUserCommandHandler
{
    private readonly ITenantRepository _tenant;

    public RegisterUserCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = User.Create(request.RoleId, request.Name, new Email(request.Email));
        await _tenant.Users.Create(user, cancellationToken);

        await _tenant.CommitAsync(cancellationToken);

        return user.Id;
    }
}