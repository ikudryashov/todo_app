using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Queries.GetTodoById;

public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Todo>
{
	private readonly IUserRepository _userRepository;
	private readonly ITodoRepository _todoRepository;

	public GetTodoByIdQueryHandler(IUserRepository userRepository, ITodoRepository todoRepository)
	{
		_userRepository = userRepository;
		_todoRepository = todoRepository;
	}

	public async Task<Todo> Handle(GetTodoByIdQuery query, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(query.UserId) is null)
		{
			throw new ApiException("Not Found", "User does not exist.", HttpStatusCode.NotFound);
		}

		if (await _todoRepository.GetTodoById(query.Id) is not Todo todo)
		{
			throw new ApiException("Not Found", "Todo does not exist.", HttpStatusCode.NotFound);
		}

		return todo;
	}
}