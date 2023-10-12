using Tracker.Domain.Users;

namespace Tracker.Domain.Requests.Workflows;

public class Step
{
    public Guid Id { get; private init; }
    public string Title { get; private init; }
    public int Order { get; private set; }
    public Guid? UserId { get; private set; }
    public Guid? RoleId { get; private set; }
    public string? Comment { get; private set; }
    public StepStatus Status { get; private set; }
    public DateTime? PlanningDate { get; private set; }

    public Step(
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

    public static Step CreateByUser(
        string title,
        int order,
        Guid? userId,
        string? comment,
        StepStatus status,
        DateTime? planningDate)
    {
        return new Step(Guid.NewGuid(), title, order, userId, null, comment, status, planningDate);
    }
    
    public static Step CreateByRole(
        string title,
        int order,
        Guid? roleId,
        string? comment,
        StepStatus status,
        DateTime? planningDate)
    {
        return new Step(Guid.NewGuid(), title, order, null, roleId, comment, status, planningDate);
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