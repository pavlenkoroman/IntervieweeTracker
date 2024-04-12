using Tracker.Application.Repositories;
using Tracker.Application.StepTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.StepTemplates.Handlers;

public class GetStepTemplateByIdQueryHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public GetStepTemplateByIdQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<StepTemplate> Handle(GetStepByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var tenant = _tenantRepositoryFactory.GetTenant();

        var steps = await tenant.StepTemplates
            .GetByIds(new[] { request.StepId }, cancellationToken)
            .ConfigureAwait(false);

        return steps.First();
    }
}