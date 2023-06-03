using MediatR;
using TodoApp.Application.Todos.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Queries.GetTodoById;

public record GetTodoByIdQuery(
		Guid Id,
		Guid UserId
	) : IRequest<TodoResult>;