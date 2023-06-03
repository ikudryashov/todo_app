using MediatR;
using TodoApp.Application.Todos.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Commands.Create;

public record CreateTodoCommand(
		Guid UserId,
		string Title,
		string? Description,
		DateTime? DueDate,
		bool IsComplete
	) : IRequest<TodoResult>;