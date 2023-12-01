using MediatR;

namespace Tracker.Application.Users.Queries;

public sealed record LoginQuery(string Email) : IRequest<Guid>;