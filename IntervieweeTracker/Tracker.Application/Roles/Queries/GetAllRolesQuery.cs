using MediatR;
using Tracker.Domain.Users;

namespace Tracker.Application.Roles.Queries;

public sealed record GetAllRolesQuery : IRequest<IReadOnlyCollection<Role>>;