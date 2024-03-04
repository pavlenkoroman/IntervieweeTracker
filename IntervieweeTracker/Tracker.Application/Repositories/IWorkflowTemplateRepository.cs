using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.Repositories;

public interface IWorkflowTemplateRepository
{
    Task Create(WorkflowTemplate workflowTemplate);
    Task Update(WorkflowTemplate workflowTemplate);

    Task<IReadOnlyCollection<WorkflowTemplate>> GetByIds(
        IReadOnlyCollection<Guid> workflowTemplateIds,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<WorkflowTemplate>> GetAll(CancellationToken cancellationToken);
}