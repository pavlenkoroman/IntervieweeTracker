using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.Repositories;

public interface IStepTemplateRepository
{
    void Create(StepTemplate stepTemplate);
    Task<StepTemplate> GetById(Guid stepTemplateId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<StepTemplate>> GetByIds(
        IReadOnlyCollection<Guid> stepTemplateIds,
        CancellationToken cancellationToken);
}