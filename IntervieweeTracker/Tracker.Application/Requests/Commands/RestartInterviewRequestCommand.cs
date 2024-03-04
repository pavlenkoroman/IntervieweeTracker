
namespace Tracker.Application.Requests.Commands;

public sealed record RestartInterviewRequestCommand(Guid RequestId, Guid UserId);