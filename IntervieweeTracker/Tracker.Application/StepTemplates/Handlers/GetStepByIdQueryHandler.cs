using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.StepTemplates.Queries;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.StepTemplates.Handlers;

public class GetStepByIdQueryHandler : IRequestHandler<GetStepByIdQuery, StepTemplate>
{
    private readonly ITenantRepository _tenant;

    public GetStepByIdQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<StepTemplate> Handle(GetStepByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _tenant.StepTemplates.GetById(request.StepId, cancellationToken);
    }
}