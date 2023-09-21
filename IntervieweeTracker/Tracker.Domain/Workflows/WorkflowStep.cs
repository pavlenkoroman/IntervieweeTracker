using Tracker.Domain.Enums;
using Tracker.Domain.UserInfo;

namespace Tracker.Domain.Workflows;

public class WorkflowStep
{
    public Guid Id { get; private init; }
    public string Title { get; private init; }
    public int Order { get; private set; }
    public Guid? UserId { get; private set; }
    public Guid? RoleId { get; private set; }
    public string? Comment { get; private set; }
    public StepStatus Status { get; private set; }
    public DateTime? PlanningDate { get; private set; }

    private WorkflowStep(
        Guid id,
        string title,
        int order,
        Guid? userId,
        Guid? roleId,
        string? comment,
        StepStatus status,
        DateTime? planningDate)
    {
        Id = id;
        Title = title;
        Order = order;
        UserId = userId;
        RoleId = roleId;
        Comment = comment;
        Status = status;
        PlanningDate = planningDate;
    }

    public static WorkflowStep Create(
        string title,
        int order,
        Guid? userId,
        Guid? roleId,
        string? comment,
        StepStatus status,
        DateTime? planningDate)
    {
        return new WorkflowStep(Guid.NewGuid(), title, order, userId, roleId, comment, status, planningDate);
    }

    public void UpdateComment(string comment)
    {
        Comment = comment;
    }

    public void Reschedule(DateTime newDate)
    {
        ArgumentNullException.ThrowIfNull(newDate);

        PlanningDate = newDate;
    }

    public void SetStatus(User user, StepStatus status)
    {
        ArgumentNullException.ThrowIfNull(user);

        UserId = user.Id;
        RoleId = user.RoleId;
        Status = status;
    }
}