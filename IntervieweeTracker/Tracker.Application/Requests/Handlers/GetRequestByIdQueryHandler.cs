using MediatR;
using Tracker.Application.Repositories;
using Tracker.Application.Requests.Queries;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Handlers;

public class GetRequestByIdQueryHandler : IRequestHandler<GetRequestByIdQuery, Request>
{
    private readonly ITenantRepository _tenant;

    public GetRequestByIdQueryHandler(ITenantRepository tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        _tenant = tenant;
    }

    public async Task<Request> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestsCollection = await _tenant.Requests.GetByIds(new[] { request.RequestId }, cancellationToken);

        return requestsCollection.Single();
    }
}