using MediatR;
using Tracker.Domain.Requests;

namespace Tracker.Application.Requests.Queries;

public sealed record GetRequestByIdQuery(Guid RequestId) : IRequest<Request>;