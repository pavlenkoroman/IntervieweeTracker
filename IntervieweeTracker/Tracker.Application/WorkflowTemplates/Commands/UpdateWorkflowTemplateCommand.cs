using MediatR;

namespace Tracker.Application.WorkflowTemplates.Commands;

public record UpdateWorkflowTemplateCommand(
        Guid WorkflowTemplateId,
        string Title,
        IReadOnlyCollection<Guid> StepTemplateIds)
    : IRequest<Guid>;