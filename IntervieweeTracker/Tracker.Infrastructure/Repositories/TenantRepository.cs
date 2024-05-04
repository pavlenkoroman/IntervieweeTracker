using Tracker.Application.Repositories;

namespace Tracker.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly DbContext _dbContext;
    public IUserRepository Users { get; }
    public IRoleRepository Roles { get; }
    public IRequestRepository Requests { get; }
    public IWorkflowTemplateRepository WorkflowTemplates { get; }
    public IStepTemplateRepository StepTemplates { get; }

    public TenantRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        Users = new UserRepository(_dbContext);
        Roles = new RoleRepository(_dbContext);
        Requests = new RequestRepository(_dbContext);
        WorkflowTemplates = new WorkflowTemplateRepository(_dbContext);
        StepTemplates = new StepTemplateRepository(_dbContext);
    }

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    public void Dispose()
    {
        _dbContext.Connection.Close();
    }
}