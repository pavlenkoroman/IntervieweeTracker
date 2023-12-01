using MediatR;

namespace Tracker.Application.Requests.Commands;

public sealed record ChangeInterviewerCommand(Guid RequestId, Guid UserId) : IRequest;