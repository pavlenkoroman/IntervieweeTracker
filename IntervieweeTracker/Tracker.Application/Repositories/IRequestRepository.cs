using Tracker.Domain.Requests;

namespace Tracker.Application.Repositories;

public interface IRequestRepository
{
    Task Create(Request request, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Request>> GetByIds(
        IReadOnlyCollection<Guid> requestIds,
        CancellationToken cancellationToken);
}