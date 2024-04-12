using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.WorkflowTemplates.Handlers;

public class GetAllWorkflowTemplatesQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetAllWorkflowTemplatesQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<IReadOnlyCollection<WorkflowTemplate>> Handle(
        GetAllWorkflowTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        return await tenant.WorkflowTemplates.GetAll(cancellationToken);
    }
}