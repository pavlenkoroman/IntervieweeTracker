namespace Tracker.Application.WorkflowTemplates.Commands;

public record UpdateWorkflowTemplateCommand(
    Guid WorkflowTemplateId,
    string Title,
    IReadOnlyCollection<Guid> StepTemplateIds);