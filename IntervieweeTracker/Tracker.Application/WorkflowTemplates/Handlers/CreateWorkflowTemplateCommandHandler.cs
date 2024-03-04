using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Commands;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.WorkflowTemplates.Handlers;

public sealed record CreateWorkflowTemplateCommandHandler
{
    private readonly ITenantRepository _tenant;

    public CreateWorkflowTemplateCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Guid> Handle(CreateWorkflowTemplateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var stepTemplates = await _tenant.StepTemplates.GetByIds(
            request.StepTemplateIds,
            cancellationToken);

        var workflowTemplate = WorkflowTemplate.Create(request.Title, stepTemplates);

        await _tenant.WorkflowTemplates.Create(workflowTemplate);

        await _tenant.CommitAsync(cancellationToken);

        return workflowTemplate.Id;
    }
}