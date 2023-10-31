﻿using Tracker.Domain.Events;
using Tracker.Domain.Requests.Events;
using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;

namespace Tracker.Domain.Requests;

public class Request
{
    private HashSet<IEvent> _events = new HashSet<IEvent>();

    public Guid Id { get; private init; }
    public Guid UserId { get; private set; }
    public Document Document { get; private init; }
    public Workflow Workflow { get; private init; }
    public IReadOnlyCollection<IEvent> Events => _events;

    public Request(
        Guid id,
        Guid userId,
        Document document,
        Workflow workflow)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(document);
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
        Document = document;
        Workflow = workflow;
    }

    public static Request Create(Guid userId, Document inervieweeDocument, Workflow workflow)
    {
        var requestId = Guid.NewGuid();

        var request = new Request(requestId, userId, inervieweeDocument, workflow);

        var onCreateEvent = RequestCreatedEvent.Create(requestId, $"{DateTime.UtcNow} - interview request added");
        
        request.AddEvent(onCreateEvent);

        return request;
    }

    public void Approve(User user)
    {
        Workflow.ApproveStep(user);
        var onApproveEvent = RequestRejectedEvent.Create(Id, $"{DateTime.UtcNow} - interview request rejected");
        AddEvent(onApproveEvent);
    }

    public void Reject(User user)
    {
        Workflow.RejectStep(user);
        var onRejectEvent = RequestRejectedEvent.Create(Id, $"{DateTime.UtcNow} - interview request rejected");
        AddEvent(onRejectEvent);
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

    private void AddEvent(IEvent requestEvent)
    {
        _events.Add(requestEvent);
    }
}