namespace Tracker.Infrastructure.DbObjects;

public sealed record StepDbObject(
    Guid Id,
    string Title,
    int Order,
    string Comment,
    int Status,
    DateTime PlanningDate,
    Guid RoleId,
    Guid UserId,
    Guid WorkflowId);