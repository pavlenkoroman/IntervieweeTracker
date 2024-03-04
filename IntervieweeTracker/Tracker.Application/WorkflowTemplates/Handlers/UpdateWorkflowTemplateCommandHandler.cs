using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Commands;

namespace Tracker.Application.WorkflowTemplates.Handlers;

public class UpdateWorkflowTemplateCommandHandler
{
    private readonly ITenantRepository _tenant;

    public UpdateWorkflowTemplateCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Guid> Handle(UpdateWorkflowTemplateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var steps = await _tenant.StepTemplates.GetByIds(
            request.StepTemplateIds,
            cancellationToken);

        var workflowTemplateCollection =
            await _tenant.WorkflowTemplates.GetByIds(new[] { request.WorkflowTemplateId }, cancellationToken);

        var workflowTemplate = workflowTemplateCollection.Single();

        if (!string.IsNullOrEmpty(request.Title))
        {
            workflowTemplate.UpdateTitle(request.Title);
        }

        workflowTemplate.UpdateSteps(steps);

        await _tenant.WorkflowTemplates.Update(workflowTemplate);

        await _tenant.CommitAsync(cancellationToken);

        return workflowTemplate.Id;
    }
}