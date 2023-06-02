using MediatR;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Queries.GetTodos;

public record GetTodosQuery(
		Guid UserId
	) : IRequest<List<Todo>>;