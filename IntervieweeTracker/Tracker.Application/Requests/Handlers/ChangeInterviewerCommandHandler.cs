using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Commands;

namespace Tracker.Application.Requests.Handlers;

public class ChangeInterviewerCommandHandler : IRequestHandler<ChangeInterviewerCommand>
{
    private readonly ITenantRepository _tenant;

    public ChangeInterviewerCommandHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task Handle(ChangeInterviewerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var interviewRequest = await _tenant.Requests.GetById(request.RequestId, cancellationToken);
        var user = await _tenant.Users.GetById(request.UserId, cancellationToken);

        interviewRequest.ChangeInterviewer(user);
        await _tenant.CommitAsync(cancellationToken);
    }
}