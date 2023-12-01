using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Queries;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Handlers;

public class GetRequestsByIdsQueryHandler : IRequestHandler<GetRequestsByIdsQuery, IReadOnlyCollection<Request>>
{
    private readonly ITenantRepository _tenant;

    public GetRequestsByIdsQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<IReadOnlyCollection<Request>> Handle(
        GetRequestsByIdsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _tenant.Requests.GetByIds(request.RequestIds, cancellationToken);
    }
}