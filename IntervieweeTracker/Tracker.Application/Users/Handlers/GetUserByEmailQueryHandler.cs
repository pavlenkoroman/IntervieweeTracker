using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Users.Queries;
using Tracker.Domain.Users;

namespace Tracker.Application.Users.Handlers;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User?>
{
    private readonly ITenantRepository _tenant;

    public GetUserByEmailQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<User?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _tenant.Users.GetByEmail(new Email(request.Email), cancellationToken);
    }
}