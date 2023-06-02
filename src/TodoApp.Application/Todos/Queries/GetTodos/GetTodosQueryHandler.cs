using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Queries.GetTodos;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, List<Todo>>
{
	private readonly IUserRepository _userRepository;
	private readonly ITodoRepository _todoRepository;

	public GetTodosQueryHandler(ITodoRepository todoRepository, IUserRepository userRepository)
	{
		_todoRepository = todoRepository;
		_userRepository = userRepository;
	}

	public async Task<List<Todo>> Handle(GetTodosQuery query, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(query.UserId) is null)
		{
			throw new ApiException("Not Found", "User does not exist.", HttpStatusCode.NotFound);
		}

		return await _todoRepository.GetTodos(query.UserId);
	}
}