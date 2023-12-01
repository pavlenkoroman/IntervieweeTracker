using MediatR;
using Tracker.Domain.Users;

namespace Tracker.Application.Users.Queries;

public sealed record GetAllUsersQuery : IRequest<IReadOnlyCollection<User>>;