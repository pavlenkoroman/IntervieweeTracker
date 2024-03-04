namespace Tracker.Application.StepTemplates.Queries;

public sealed record GetStepsByIdsQuery(IReadOnlyCollection<Guid> StepIds);