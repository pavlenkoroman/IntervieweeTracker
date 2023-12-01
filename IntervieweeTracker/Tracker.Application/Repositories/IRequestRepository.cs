using Tracker.Domain.Requests;

namespace Tracker.Application.Repositories;

public interface IRequestRepository
{
    void Create(Request request);
    Task<Request> GetById(Guid requestId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Request>> GetByIds(
        IReadOnlyCollection<Guid> requestIds,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Request>> GetAll(CancellationToken cancellationToken);
}