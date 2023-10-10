﻿using Tracker.Domain.UserInfo;

namespace Tracker.Domain.Request.Workflows;

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
        var stepToApprove = Steps.FirstOrDefault(step => step.Status == WorkflowStepStatus.Pending);

        if (stepToApprove is null)
        {
            throw new ArgumentException("Pending steps not found", nameof(stepToApprove));
        }

        stepToApprove.SetStatus(user, WorkflowStepStatus.Approved);
    }

    public void RejectStep(User user)
    {
        var stepToApprove = Steps.FirstOrDefault(step => step.Status == WorkflowStepStatus.Pending);

        if (stepToApprove is null)
        {
            throw new ArgumentException("Pending steps not found", nameof(stepToApprove));
        }

        stepToApprove.SetStatus(user, WorkflowStepStatus.Rejected);
    }

    public void RestartWorkflow(User user)
    {
        foreach (var step in Steps)
        {
            step.SetStatus(user, WorkflowStepStatus.Pending);
        }
    }
}