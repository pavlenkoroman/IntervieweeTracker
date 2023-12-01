using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Queries;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Handlers;

public class GetAllRequestsQueryHandler : IRequestHandler<GetAllRequestsQuery, IReadOnlyCollection<Request>>
{
    private readonly ITenantRepository _tenant;

    public GetAllRequestsQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<IReadOnlyCollection<Request>> Handle(
        GetAllRequestsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _tenant.Requests.GetAll(cancellationToken);
    }
}