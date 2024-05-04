using Dapper;
using Tracker.Application.Repositories;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Infrastructure.Repositories;

public class StepTemplateRepository : IStepTemplateRepository
{
    private readonly DbContext _dbContext;

    public StepTemplateRepository(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<StepTemplate>> GetByIds(IReadOnlyCollection<Guid> stepTemplateIds,
        CancellationToken cancellationToken)
    {
        await using var connection = _dbContext.Connection;
        var parameters = new DynamicParameters(stepTemplateIds);
        const string sql = "SELECT * FROM users WHERE id IN @StepTemplateIds";
        var result = await connection.QueryAsync<StepTemplate>(sql, parameters);

        return result.ToArray();
    }
}