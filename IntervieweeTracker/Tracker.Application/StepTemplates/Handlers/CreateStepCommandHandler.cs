using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.StepTemplates.Commands;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.StepTemplates.Handlers;

public class CreateStepCommandHandler : IRequestHandler<CreateStepTemplateCommand>
{
    private readonly ITenantRepository _tenant;

    public CreateStepCommandHandler(ITenantRepository tenant)
    {
        _tenant = tenant;
    }

    public async Task Handle(CreateStepTemplateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        StepTemplate stepTemplate;

        if (request.UserId != null)
        {
            stepTemplate = StepTemplate.CreateByUser(request.Title, request.Order, request.UserId);
        }
        else if (request.RoleId != null)
        {
            stepTemplate = StepTemplate.CreateByRole(request.Title, request.Order, request.RoleId);
        }
        else
        {
            throw new ArgumentException(
                "At least one between User Id and Role Id must be not null",
                nameof(request.UserId) + " " + nameof(request.RoleId));
        }

        await _tenant.StepTemplates.Create(stepTemplate);

        await _tenant.CommitAsync(cancellationToken);
    }
}