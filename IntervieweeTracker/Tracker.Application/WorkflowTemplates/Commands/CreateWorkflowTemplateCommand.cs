namespace Tracker.Application.WorkflowTemplates.Commands;

public sealed record CreateWorkflowTemplateCommand(string Title, IReadOnlyCollection<Guid> StepTemplateIds);