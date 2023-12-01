namespace Tracker.Application.Repositories;

public interface ITenantRepository
{
    IUserRepository Users { get; init; }
    IRoleRepository Roles { get; init; }
    IRequestRepository Requests { get; init; }
    IWorkflowTemplateRepository WorkflowTemplates { get; init; }
    IStepTemplateRepository StepTemplates { get; init; }
    Task CommitAsync(CancellationToken cancellationToken);
}