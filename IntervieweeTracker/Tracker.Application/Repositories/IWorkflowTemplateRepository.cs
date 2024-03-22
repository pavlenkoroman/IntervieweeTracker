using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.Repositories;

public interface IWorkflowTemplateRepository
{
    Task Create(WorkflowTemplate workflowTemplate, CancellationToken cancellationToken);
    Task Update(WorkflowTemplate workflowTemplate, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<WorkflowTemplate>> GetByIds(
        IReadOnlyCollection<Guid> workflowTemplateIds,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<WorkflowTemplate>> GetAll(CancellationToken cancellationToken);
}