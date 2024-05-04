using Dapper;
using Tracker.Application.Repositories;
using Tracker.Domain.Users;

namespace Tracker.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DbContext _dbContext;

    public RoleRepository(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task Create(Role role, CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;
        const string sql = "INSERT INTO roles(id, text) VALUES(@Id, @Title)";
        await connection.ExecuteAsync(sql, role);
    }

    public async Task<IReadOnlyCollection<Role>> GetByIds(
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;
        var parameters = new DynamicParameters(roleIds);
        const string sql = "SELECT * FROM users WHERE id IN @RoleIds";
        var result = await connection.QueryAsync<Role>(sql, parameters);

        return result.ToArray();
    }
}