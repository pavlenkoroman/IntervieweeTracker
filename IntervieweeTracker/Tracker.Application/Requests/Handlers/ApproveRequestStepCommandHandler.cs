using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class ApproveRequestStepCommandHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public ApproveRequestStepCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task Handle(ApproveRequestStepCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        var interviewRequest = await tenant.Requests.GetByIds(
                new[] { request.RequestId },
                cancellationToken)
            .ConfigureAwait(false);

        var user = await tenant.Users
            .GetByIds(new[] { request.UserId }, cancellationToken)
            .ConfigureAwait(false);

        interviewRequest.Single().Approve(user.Single());
        await tenant.CommitAsync(cancellationToken);
    }
}