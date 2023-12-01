using MediatR;
using Tracker.Domain.Users;

namespace Tracker.Application.Users.Queries;

public sealed record GetUserByEmailQuery(string Email) : IRequest<User?>;