using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Guid>
{
    private readonly ITenantRepository _tenant;

    public CreateRequestCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var workflowTemplate = await _tenant.WorkflowTemplates.GetById(request.WorkflowTemplateId, cancellationToken);
        var user = await _tenant.Users.GetById(request.UserId, cancellationToken);
        var interviewRequest = workflowTemplate.CreateRequest(user, request.Document);

        _tenant.Requests.Create(interviewRequest);

        await _tenant.CommitAsync(cancellationToken);

        return interviewRequest.Id;
    }
}