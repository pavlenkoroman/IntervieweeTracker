using Tracker.Application.Repositories;
using Tracker.Application.StepTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.StepTemplates.Handlers;

public class GetStepsByIdsQueryHandler
{
    private readonly ITenantRepository _tenant;

    public GetStepsByIdsQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<IReadOnlyCollection<StepTemplate>> Handle(
        GetStepsByIdsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _tenant.StepTemplates.GetByIds(request.StepIds, cancellationToken);
    }
}