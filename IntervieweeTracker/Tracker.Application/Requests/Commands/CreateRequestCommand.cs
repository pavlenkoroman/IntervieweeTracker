using MediatR;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Commands;

public record CreateRequestCommand(Guid WorkflowTemplateId, Guid UserId, Document Document) : IRequest<Guid>;