using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.Repositories;

public interface IStepTemplateRepository
{
    Task Create(StepTemplate stepTemplate);
    Task<IReadOnlyCollection<StepTemplate>> GetByIds(
        IReadOnlyCollection<Guid> stepTemplateIds,
        CancellationToken cancellationToken);
}