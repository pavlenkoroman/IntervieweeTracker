using Tracker.Application.Repositories;

namespace Tracker.Infrastructure.Repositories;

public class TenantRepositoryFactory : ITenantRepositoryFactory
{
    private readonly DbContext _dbContext;

    public TenantRepositoryFactory(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public ITenantRepository GetTenant()
    {
        return new TenantRepository(_dbContext);
    }
}