namespace Tracker.Infrastructure.DbObjects;

public record RequestDbObject(
    Guid Id,
    Guid UserId,
    string CandidateName,
    string CandidateEmail,
    string CandidateResume,
    Guid WorkflowId,
    Guid WorkflowTemplateId,
    string WorkflowTitle);