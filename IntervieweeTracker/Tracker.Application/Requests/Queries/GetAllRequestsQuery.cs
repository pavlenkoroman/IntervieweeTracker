using MediatR;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Queries;

public sealed record GetAllRequestsQuery : IRequest<IReadOnlyCollection<Request>>;