using MediatR;

namespace Tracker.Application.WorkflowTemplates.Commands;

public sealed record CreateWorkflowTemplateCommand(string Title, IReadOnlyCollection<Guid> StepTemplateIds)
    : IRequest<Guid>;