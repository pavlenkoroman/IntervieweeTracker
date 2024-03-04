namespace Tracker.Application.Requests.Commands;

public sealed record RejectRequestCommand(Guid RequestId, Guid UserId);