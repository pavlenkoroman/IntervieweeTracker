using Dapper;
using Tracker.Application.Repositories;
using Tracker.Domain.Users;

namespace Tracker.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbContext _dbContext;

    public UserRepository(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task Create(User user, CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;
        const string sql = "INSERT INTO users(id, role_id, name, email) VALUES(@Id, @RoleId, @Name, @Email)";
        await connection.ExecuteAsync(sql, user);
    }

    public async Task<User?> GetByEmail(Email email, CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;
        const string sql = "SELECT * FROM users WHERE users.id == @Value";
        var result = await connection.QuerySingleOrDefaultAsync<User>(sql, email);

        return result;
    }

    public async Task<IReadOnlyCollection<User>> GetByIds(
        IReadOnlyCollection<Guid> userIds,
        CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;
        var parameters = new DynamicParameters(userIds);
        const string sql = "SELECT * FROM users WHERE id IN @UserIds";
        var result = await connection.QueryAsync<User>(sql, parameters);

        return result.ToArray();
    }
}