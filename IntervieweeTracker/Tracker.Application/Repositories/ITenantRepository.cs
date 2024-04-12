namespace Tracker.Application.Repositories;

public interface ITenantRepository : IDisposable
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IRequestRepository Requests { get; }
    IWorkflowTemplateRepository WorkflowTemplates { get; }
    IStepTemplateRepository StepTemplates { get; }
    Task CommitAsync(CancellationToken cancellationToken);
}