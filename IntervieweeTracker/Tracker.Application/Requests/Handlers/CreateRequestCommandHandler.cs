using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class CreateRequestCommandHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public CreateRequestCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        using var tenant = _tenantRepositoryFactory.GetTenant();

        var workflowTemplate = await tenant.WorkflowTemplates
            .GetByIds(new[] { request.WorkflowTemplateId }, cancellationToken)
            .ConfigureAwait(false);

        var user = await tenant.Users
            .GetByIds(new[] { request.UserId }, cancellationToken)
            .ConfigureAwait(false);

        var interviewRequest = workflowTemplate.Single().CreateRequest(user.Single(), request.Document);

        await tenant.Requests.Create(interviewRequest, cancellationToken);

        await tenant.CommitAsync(cancellationToken);

        return interviewRequest.Id;
    }
}