using Tracker.Application.Repositories;
using Tracker.Application.WorkflowTemplates.Commands;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.WorkflowTemplates.Handlers;

public sealed record CreateWorkflowTemplateCommandHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public CreateWorkflowTemplateCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Guid> Handle(CreateWorkflowTemplateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        var stepTemplates = await tenant.StepTemplates.GetByIds(
            request.StepTemplateIds,
            cancellationToken);

        var workflowTemplate = WorkflowTemplate.Create(request.Title, stepTemplates);

        await tenant.WorkflowTemplates.Create(workflowTemplate, cancellationToken);

        await tenant.CommitAsync(cancellationToken);

        return workflowTemplate.Id;
    }
}