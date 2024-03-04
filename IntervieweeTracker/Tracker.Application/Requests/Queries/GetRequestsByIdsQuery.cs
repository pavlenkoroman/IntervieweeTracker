namespace Tracker.Application.Requests.Queries;

public sealed record GetRequestsByIdsQuery(IReadOnlyCollection<Guid> RequestIds);