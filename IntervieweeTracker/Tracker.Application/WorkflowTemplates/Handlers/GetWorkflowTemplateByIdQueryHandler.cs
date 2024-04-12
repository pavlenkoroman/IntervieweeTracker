using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.WorkflowTemplates.Handlers;

public class GetWorkflowTemplateByIdQueryHandler
{
    private readonly ITenantRepositoryFactory _tenant;

    public GetWorkflowTemplateByIdQueryHandler(ITenantRepositoryFactory tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<WorkflowTemplate> Handle(
        GetWorkflowTemplateByIdQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenant.GetTenant();

        var workflowTemplateCollection =
            await tenant.WorkflowTemplates.GetByIds(new[] { request.WorkflowTemplateId }, cancellationToken);

        return workflowTemplateCollection.Single();
    }
}