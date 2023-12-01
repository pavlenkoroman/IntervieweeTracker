using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Users.Queries;
using Tracker.Domain.Users;

namespace Tracker.Application.Users.Handlers;

public class LoginQueryHandler : IRequestHandler<LoginQuery, Guid>
{
    private readonly ITenantRepository _tenant;

    public LoginQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Guid> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await _tenant.Users.GetByEmail(new Email(request.Email), cancellationToken);

        if (user is null) throw new ArgumentException("Current user does not exist", nameof(user));

        await _tenant.Users.Login(user, cancellationToken);

        return user.Id;
    }
}