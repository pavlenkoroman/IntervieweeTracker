﻿using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;

namespace Tracker.Domain.Requests;

public class Request
{
    public Guid Id { get; private init; }
    public Guid UserId { get; private set; }
    public Document Document { get; private init; }
    public Workflow Workflow { get; private init; }

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

        return request;
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
}