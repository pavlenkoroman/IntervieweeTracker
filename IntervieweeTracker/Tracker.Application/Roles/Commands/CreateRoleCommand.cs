using MediatR;

namespace Tracker.Application.Roles.Commands;

public sealed record CreateRoleCommand(string Title) : IRequest<Guid>;