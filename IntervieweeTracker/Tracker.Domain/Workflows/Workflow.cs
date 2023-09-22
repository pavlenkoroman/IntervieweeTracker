using Tracker.Domain.Enums;
using Tracker.Domain.UserInfo;

namespace Tracker.Domain.Workflows;

public class Workflow
{
    public Guid Id { get; private init; }
    public Guid WorkflowTemplateId { get; private init; }
    public string Title { get; private set; }
    public IReadOnlyCollection<WorkflowStep> Steps { get; private set; }

    public Workflow(Guid id, Guid workflowTemplateId, string title, IReadOnlyCollection<WorkflowStep> steps)
    {
        Id = id;
        WorkflowTemplateId = workflowTemplateId;
        Title = title;
        Steps = steps;
    }

    public static Workflow Create(Guid workflowTemplateId, string title, IReadOnlyCollection<WorkflowStep> steps)
    {
        return new Workflow(Guid.NewGuid(), workflowTemplateId, title, steps);
    }

    public void ApproveStep(User user)
    {
        var stepToApprove = Steps.FirstOrDefault(step => step.Status == StepStatus.Pending);

        if (stepToApprove is null)
        {
            throw new ArgumentException("Pending steps not found", nameof(stepToApprove));
        }

        stepToApprove.SetStatus(user, StepStatus.Approved);
    }

    public void RejectStep(User user)
    {
        var stepToApprove = Steps.FirstOrDefault(step => step.Status == StepStatus.Pending);

        if (stepToApprove is null)
        {
            throw new ArgumentException("Pending steps not found", nameof(stepToApprove));
        }

        stepToApprove.SetStatus(user, StepStatus.Rejected);
    }

    public void RestartWorkflow(User user)
    {
        foreach (var step in Steps)
        {
            step.SetStatus(user, StepStatus.Pending);
        }
    }
}