using MediatR;

namespace Tracker.Application.Requests.Commands;

public sealed record ApproveRequestStepCommand(Guid RequestId, Guid UserId) : IRequest;