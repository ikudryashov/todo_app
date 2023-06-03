using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Todos.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Queries.GetTodos;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, List<TodoResult>>
{
	private readonly IUserRepository _userRepository;
	private readonly ITodoRepository _todoRepository;

	public GetTodosQueryHandler(ITodoRepository todoRepository, IUserRepository userRepository)
	{
		_todoRepository = todoRepository;
		_userRepository = userRepository;
	}

	public async Task<List<TodoResult>> Handle(GetTodosQuery query, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(query.UserId) is null)
		{
			throw new ApiException("Not Found", "User does not exist.", HttpStatusCode.NotFound);
		}
		
		var todos = await _todoRepository.GetTodos(query.UserId);
		var result = new List<TodoResult>();

		foreach (var todo in todos)
		{
			if (todo is not null)
			{
				result.Add(new TodoResult(todo.Id, todo.Title, todo.Description, todo.DueDate, todo.IsComplete));
			}
		}

		return result;
	}
}