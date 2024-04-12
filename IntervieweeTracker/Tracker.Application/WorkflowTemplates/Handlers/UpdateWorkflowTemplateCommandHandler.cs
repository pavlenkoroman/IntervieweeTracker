using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Commands;

namespace Tracker.Application.WorkflowTemplates.Handlers;

public class UpdateWorkflowTemplateCommandHandler
{
    private readonly ITenantRepositoryFactory _tenant;

    public UpdateWorkflowTemplateCommandHandler(ITenantRepositoryFactory tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Guid> Handle(UpdateWorkflowTemplateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenant.GetTenant();

        var steps = await tenant.StepTemplates.GetByIds(request.StepTemplateIds, cancellationToken);

        var workflowTemplateCollection =
            await tenant.WorkflowTemplates.GetByIds(new[] { request.WorkflowTemplateId }, cancellationToken);

        var workflowTemplate = workflowTemplateCollection.Single();

        if (!string.IsNullOrEmpty(request.Title))
        {
            workflowTemplate.UpdateTitle(request.Title);
        }

        workflowTemplate.UpdateSteps(steps);

        await tenant.WorkflowTemplates.Update(workflowTemplate, cancellationToken);

        await tenant.CommitAsync(cancellationToken);

        return workflowTemplate.Id;
    }
}