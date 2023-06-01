using MediatR;

namespace TodoApp.Application.Users.Commands.Delete;

public record DeleteUserCommand(
		Guid Id
	) : IRequest;