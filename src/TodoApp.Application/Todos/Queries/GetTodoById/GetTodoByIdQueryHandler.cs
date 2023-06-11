using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Todos.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Queries.GetTodoById;

public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, TodoResult>
{
	private readonly IUserRepository _userRepository;
	private readonly ITodoRepository _todoRepository;

	public GetTodoByIdQueryHandler(IUserRepository userRepository, ITodoRepository todoRepository)
	{
		_userRepository = userRepository;
		_todoRepository = todoRepository;
	}

	public async Task<TodoResult> Handle(GetTodoByIdQuery query, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(query.UserId) is not User user)
		{
			throw new ApiException("Not Found", "User does not exist.", nameof(GetTodoByIdQuery),
				HttpStatusCode.NotFound);
		}

		if (await _todoRepository.GetTodoById(query.Id) is not Todo todo)
		{
			throw new ApiException("Not Found", "Todo does not exist.", nameof(GetTodoByIdQuery),
				HttpStatusCode.NotFound);
		}

		if (todo.UserId != user.Id)
		{
			throw new ApiException("Unauthorized", "You do not have access to this resource.", 
				nameof(GetTodoByIdQuery),
				HttpStatusCode.Unauthorized);
		}

		return new TodoResult(todo.Id, todo.UserId, todo.Title, todo.Description, todo.DueDate, todo.IsComplete);
	}
}