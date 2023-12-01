using MediatR;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.StepTemplates.Queries;

public sealed record GetStepByIdQuery(Guid StepId) : IRequest<StepTemplate>;