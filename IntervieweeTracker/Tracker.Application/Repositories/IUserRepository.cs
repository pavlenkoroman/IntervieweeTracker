using Tracker.Domain.Users;

namespace Tracker.Application.Repositories;

public interface IUserRepository
{
    void Create(User user);
    Task Login(User user, CancellationToken cancellationToken);
    Task<User> GetByEmail(Email email, CancellationToken cancellationToken);
    Task<User> GetById(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<User>> GetAll(CancellationToken cancellationToken);
}