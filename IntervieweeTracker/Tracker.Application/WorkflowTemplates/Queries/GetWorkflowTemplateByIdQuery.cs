using MediatR;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Application.WorkflowTemplates.Queries;

public sealed record GetWorkflowTemplateByIdQuery(Guid WorkflowTemplateId) : IRequest<WorkflowTemplate>;