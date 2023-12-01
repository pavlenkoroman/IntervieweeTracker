using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.Repositories;

public interface IWorkflowTemplateRepository
{
    void Create(WorkflowTemplate workflowTemplate);
    Task Update(WorkflowTemplate workflowTemplate);
    Task<WorkflowTemplate> GetById(Guid workflowTemplateId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<WorkflowTemplate>> GetAll(CancellationToken cancellationToken);
}