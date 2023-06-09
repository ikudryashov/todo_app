using MediatR;

namespace TodoApp.Application.Users.Commands.Update;

public record UpdateUserCommand(
		Guid Id,
		Guid RequestId,
		string FirstName,
		string LastName,
		string Email
) : IRequest<Unit>;