using MediatR;
using Tracker.Domain.Users;

namespace Tracker.Application.Roles.Queries;

public sealed record GetRoleByIdQuery(Guid RoleId) : IRequest<Role>;