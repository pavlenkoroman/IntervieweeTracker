using Tracker.Application.Repositories;
using Tracker.Application.StepTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.StepTemplates.Handlers;

public class GetStepTemplatesByIdsQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetStepTemplatesByIdsQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<IReadOnlyCollection<StepTemplate>> Handle(
        GetStepsByIdsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();
        return await tenant.StepTemplates.GetByIds(request.StepIds, cancellationToken);
    }
}