namespace Tracker.Application.Users.Commands;

public sealed record RegisterUserCommand(Guid RoleId, string Name, string Email, string Password);