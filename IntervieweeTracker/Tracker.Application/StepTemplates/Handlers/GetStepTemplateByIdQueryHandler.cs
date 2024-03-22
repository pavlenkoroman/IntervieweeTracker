using Tracker.Application.Repositories;
using Tracker.Application.StepTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.StepTemplates.Handlers;

public class GetStepTemplateByIdQueryHandler
{
    private readonly ITenantRepository _tenant;

    public GetStepTemplateByIdQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<StepTemplate> Handle(GetStepByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var steps = await _tenant.StepTemplates
            .GetByIds(new[] { request.StepId }, cancellationToken)
            .ConfigureAwait(false);

        return steps.First();
    }
}