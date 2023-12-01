using MediatR;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Queries;

public sealed record GetRequestsByIdsQuery(IReadOnlyCollection<Guid> RequestIds)
    : IRequest<IReadOnlyCollection<Request>>;