using MediatR;

namespace Tracker.Application.StepTemplates.Commands;

public sealed record CreateStepTemplateCommand(string Title, int Order, Guid? UserId, Guid? RoleId) : IRequest;