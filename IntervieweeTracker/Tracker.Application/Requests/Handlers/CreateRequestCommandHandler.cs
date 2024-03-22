using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class CreateRequestCommandHandler
{
    private readonly ITenantRepository _tenant;

    public CreateRequestCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var workflowTemplate = await _tenant.WorkflowTemplates
            .GetByIds(new[] { request.WorkflowTemplateId }, cancellationToken)
            .ConfigureAwait(false);

        var user = await _tenant.Users
            .GetByIds(new[] { request.UserId }, cancellationToken)
            .ConfigureAwait(false);

        var interviewRequest = workflowTemplate.Single().CreateRequest(user.Single(), request.Document);

        await _tenant.Requests.Create(interviewRequest, cancellationToken);

        await _tenant.CommitAsync(cancellationToken);

        return interviewRequest.Id;
    }
}