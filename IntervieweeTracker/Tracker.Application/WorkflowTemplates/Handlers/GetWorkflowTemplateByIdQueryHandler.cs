using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.WorkflowTemplates.Handlers;

public class GetWorkflowTemplateByIdQueryHandler
{
    private readonly ITenantRepository _tenant;

    public GetWorkflowTemplateByIdQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<WorkflowTemplate> Handle(
        GetWorkflowTemplateByIdQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var workflowTemplateCollection =
            await _tenant.WorkflowTemplates.GetByIds(new[] { request.WorkflowTemplateId }, cancellationToken);

        return workflowTemplateCollection.Single();
    }
}