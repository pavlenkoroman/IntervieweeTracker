using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;
using Tracker.Application.WorkflowTemplates.Commands;

namespace Tracker.Application.WorkflowTemplates.Handlers;

public class UpdateWorkflowTemplateCommandHandler : IRequestHandler<UpdateWorkflowTemplateCommand, Guid>
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

        var workflowTemplate = await _tenant.WorkflowTemplates.GetById(request.WorkflowTemplateId, cancellationToken);

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