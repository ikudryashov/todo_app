using MediatR;

namespace TodoApp.Application.Todos.Commands.Update;

public record UpdateTodoCommand(
		Guid Id,
		Guid UserId,
		string Title,
		string? Description,
		DateTime? DueDate,
		bool IsComplete
	) : IRequest<Unit>;