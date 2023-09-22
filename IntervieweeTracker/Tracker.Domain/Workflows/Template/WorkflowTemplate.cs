using Tracker.Domain.Enums;
using Tracker.Domain.Request;
using Tracker.Domain.UserInfo;

namespace Tracker.Domain.Workflows.Template;

public class WorkflowTemplate
{
    public Guid Id { get; private init; }
    public string Title { get; private set; }
    public IReadOnlyCollection<WorkflowStepTemplate> Steps { get; private set; }

    public WorkflowTemplate(Guid id, string title, IReadOnlyCollection<WorkflowStepTemplate> steps)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(steps);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("Guid cannot be empty", nameof(id));
        }

        if (steps.Count == 0)
        {
            throw new ArgumentException("Workflow template must contain at least one step", nameof(steps));
        }

        Id = id;
        Title = title;
        Steps = steps;
    }

    public static WorkflowTemplate Create(string title, IReadOnlyCollection<WorkflowStepTemplate> steps)
    {
        return new WorkflowTemplate(Guid.NewGuid(), title, steps);
    }

    public InterviewRequest CreateRequest(User user, IntervieweeDocument document)
    {
        return InterviewRequest.Create(
            user.Id,
            document,
            Workflow.Create(
                Id,
                Title,
                Steps.Select(stepTemplate =>
                        WorkflowStep.Create(
                            stepTemplate.Title,
                            stepTemplate.Order,
                            user.Id,
                            user.RoleId,
                            null,
                            StepStatus.Pending,
                            null))
                    .ToList()));
    }
}