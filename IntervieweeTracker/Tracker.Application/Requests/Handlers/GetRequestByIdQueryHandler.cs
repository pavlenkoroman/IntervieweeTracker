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

        return await _tenant.Requests.GetById(request.RequestId, cancellationToken);
    }
}