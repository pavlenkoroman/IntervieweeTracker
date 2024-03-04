using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.WorkflowTemplates.Handlers;

public class GetAllWorkflowTemplatesQueryHandler
{
    private readonly ITenantRepository _tenant;

    public GetAllWorkflowTemplatesQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<IReadOnlyCollection<WorkflowTemplate>> Handle(
        GetAllWorkflowTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _tenant.WorkflowTemplates.GetAll(cancellationToken);
    }
}