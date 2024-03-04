using Tracker.Domain.Requests;

namespace Tracker.Application.Repositories;

public interface IRequestRepository
{
    Task Create(Request request);

    Task<IReadOnlyCollection<Request>> GetByIds(
        IReadOnlyCollection<Guid> requestIds,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Request>> GetAll(CancellationToken cancellationToken);
}