using MediatR;

namespace TodoApp.Application.Users.Commands.Update;

public record UpdateUserCommand(
		Guid Id,
		string FirstName,
		string LastName,
		string Email
) : IRequest<Unit>;