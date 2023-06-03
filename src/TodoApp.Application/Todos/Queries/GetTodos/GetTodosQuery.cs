using MediatR;
using TodoApp.Application.Todos.Common;

namespace TodoApp.Application.Todos.Queries.GetTodos;

public record GetTodosQuery(
		Guid UserId
	) : IRequest<List<TodoResult>>;