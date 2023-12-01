using Tracker.Domain.Users;

namespace Tracker.Application.Repositories;

public interface IRoleRepository
{
    void Create(Role role);
    Task<Role> GetById(Guid roleId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Role>> GetAll(CancellationToken cancellationToken);
}