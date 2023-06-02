using MediatR;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Commands.Create;

public record CreateTodoCommand(
		Guid UserId,
		string Title,
		string? Description,
		DateTime? DueDate
	) : IRequest<Todo>;