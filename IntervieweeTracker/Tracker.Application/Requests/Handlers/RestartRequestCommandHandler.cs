using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class RestartRequestCommandHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public RestartRequestCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task Handle(RestartRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        var interviewRequests = await tenant.Requests.GetByIds(new[] { request.RequestId }, cancellationToken);
        var users = await tenant.Users.GetByIds(new[] { request.UserId }, cancellationToken);

        interviewRequests.Single().RestartInterview(users.Single());
        await tenant.CommitAsync(cancellationToken);
    }
}