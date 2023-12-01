using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Roles.Commands;
using Tracker.Domain.Users;

namespace Tracker.Application.Roles.Handlers;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly ITenantRepository _tenant;

    public CreateRoleCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var role = Role.Create(request.Title);

        _tenant.Roles.Create(role);
        await _tenant.CommitAsync(cancellationToken);

        return role.Id;
    }
}