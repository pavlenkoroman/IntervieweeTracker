using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class RestartRequestCommandHandler
{
    private readonly ITenantRepository _tenant;

    public RestartRequestCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task Handle(RestartRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var interviewRequests = await _tenant.Requests.GetByIds(new[] { request.RequestId }, cancellationToken);
        var users = await _tenant.Users.GetByIds(new[] { request.UserId }, cancellationToken);

        interviewRequests.Single().RestartInterview(users.Single());
        await _tenant.CommitAsync(cancellationToken);
    }
}