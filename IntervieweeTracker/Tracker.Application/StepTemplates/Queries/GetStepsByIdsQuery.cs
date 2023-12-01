using MediatR;
using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.StepTemplates.Queries;

public sealed record GetStepsByIdsQuery(IReadOnlyCollection<Guid> StepIds)
    : IRequest<IReadOnlyCollection<StepTemplate>>;