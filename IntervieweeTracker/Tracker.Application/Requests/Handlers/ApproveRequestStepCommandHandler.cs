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

        var interviewRequest = await _tenant.Requests.GetByIds(
                new[] { request.RequestId },
                cancellationToken)
            .ConfigureAwait(false);

        var user = await _tenant.Users
            .GetByIds(new[] { request.UserId }, cancellationToken)
            .ConfigureAwait(false);

        interviewRequest.Single().Approve(user.Single());
        await _tenant.CommitAsync(cancellationToken);
    }
}