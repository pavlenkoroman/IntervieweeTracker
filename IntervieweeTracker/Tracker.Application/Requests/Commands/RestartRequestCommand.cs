
namespace Tracker.Application.Requests.Commands;

public sealed record RestartRequestCommand(Guid RequestId, Guid UserId);