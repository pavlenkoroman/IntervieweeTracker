using Tracker.Application.Repositories;
using Tracker.Application.StepTemplates.Commands;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.StepTemplates.Handlers;

public class CreateStepTemplateCommandHandler
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public CreateStepTemplateCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<Guid> Handle(CreateStepTemplateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        StepTemplate stepTemplate;

        if (request.UserId != null)
        {
            stepTemplate = StepTemplate.CreateByUser(request.Title, request.Order, (Guid)request.UserId);
        }
        else if (request.RoleId != null)
        {
            stepTemplate = StepTemplate.CreateByRole(request.Title, request.Order, (Guid)request.RoleId);
        }
        else
        {
            throw new ArgumentException(
                "At least one between User Id and Role Id must be not null",
                nameof(request.UserId) + " " + nameof(request.RoleId));
        }

        using var tenant = _tenantRepositoryFactory.GetTenant();

        await tenant.StepTemplates.Create(stepTemplate, cancellationToken);

        await tenant.CommitAsync(cancellationToken);

        return stepTemplate.Id;
    }
}