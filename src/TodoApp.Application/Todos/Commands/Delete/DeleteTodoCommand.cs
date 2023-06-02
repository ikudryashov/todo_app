using MediatR;

namespace TodoApp.Application.Todos.Commands.Delete;

public record DeleteTodoCommand(
		Guid Id,
		Guid UserId
	) : IRequest<Unit>;