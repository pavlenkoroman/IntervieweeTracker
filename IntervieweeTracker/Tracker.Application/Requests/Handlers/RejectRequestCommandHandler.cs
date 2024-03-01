using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class RejectRequestCommandHandler : IRequestHandler<RejectRequestCommand>
{
    private readonly ITenantRepository _tenant;

    public RejectRequestCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task Handle(RejectRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var interviewRequestCollection = await _tenant.Requests
            .GetByIds(new[] { request.RequestId }, cancellationToken)
            .ConfigureAwait(false);

        var userCollection = await _tenant.Users.GetByIds(new[] { request.UserId }, cancellationToken);

        interviewRequestCollection.Single().Reject(userCollection.Single());
        await _tenant.CommitAsync(cancellationToken);
    }
}