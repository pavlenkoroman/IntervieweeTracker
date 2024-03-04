using Tracker.Domain.Users;

namespace Tracker.Application.Repositories;

public interface IRoleRepository
{
    Task Create(Role role);
    Task<IReadOnlyCollection<Role>> GetByIds(IReadOnlyCollection<Guid> roleIds, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Role>> GetAll(CancellationToken cancellationToken);
}