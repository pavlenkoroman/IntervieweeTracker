using Tracker.Domain.Requests.Events;
using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;

namespace Tracker.Domain.Requests;

public class InterviewRequest
{
    public Guid Id { get; private init; }
    public Guid UserId { get; private set; }
    public IntervieweeDocument IntervieweeDocument { get; private init; }
    public Workflow Workflow { get; private init; }
    public IReadOnlyCollection<IEvent> Events { get; private init; }

    public InterviewRequest(
        Guid id,
        Guid userId,
        IntervieweeDocument intervieweeDocument,
        Workflow workflow,
        IReadOnlyCollection<IEvent> events)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(intervieweeDocument);
        ArgumentNullException.ThrowIfNull(workflow);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("Guid cannot be empty", nameof(id));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty", nameof(userId));
        }

        Id = id;
        UserId = userId;
        IntervieweeDocument = intervieweeDocument;
        Workflow = workflow;
        Events = events;
    }

    public static InterviewRequest Create(Guid userId, IntervieweeDocument inervieweeDocument, Workflow workflow)
    {
        var requestId = Guid.NewGuid();

        return new InterviewRequest(requestId,
            userId,
            inervieweeDocument,
            workflow,
            new List<IEvent>
            {
                RequestCreatedEvent.Create(requestId, $"{DateTime.UtcNow} - interview request added")
            });
    }

    public void Approve(User user)
    {
        Workflow.ApproveStep(user);
    }

    public void Reject(User user)
    {
        Workflow.RejectStep(user);
    }

    public void RestartInterview(User user)
    {
        Workflow.RestartWorkflow(user);
    }

    public void ChangeInterviewer(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        UserId = user.Id;
    }
}