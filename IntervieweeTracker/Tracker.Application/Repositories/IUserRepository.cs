using Tracker.Domain.Users;

namespace Tracker.Application.Repositories;

public interface IUserRepository
{
    Task Create(User user, CancellationToken cancellationToken);
    Task<User?> GetByEmail(Email email, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<User>> GetByIds(IReadOnlyCollection<Guid> userIds, CancellationToken cancellationToken);
}