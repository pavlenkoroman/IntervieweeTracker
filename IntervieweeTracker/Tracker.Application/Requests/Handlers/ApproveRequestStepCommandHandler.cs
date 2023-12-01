using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class ApproveRequestStepCommandHandler : IRequestHandler<ApproveRequestStepCommand>
{
    private readonly ITenantRepository _tenant;

    public ApproveRequestStepCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task Handle(ApproveRequestStepCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var interviewRequest = await _tenant.Requests.GetById(request.RequestId, cancellationToken);
        var user = await _tenant.Users.GetById(request.UserId, cancellationToken);

        interviewRequest.Approve(user);
        await _tenant.CommitAsync(cancellationToken);
    }
}