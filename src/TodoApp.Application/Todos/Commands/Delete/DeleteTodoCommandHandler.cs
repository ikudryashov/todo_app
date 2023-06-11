using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Commands.Delete;

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, Unit>
{
	private readonly IUserRepository _userRepository;
	private readonly ITodoRepository _todoRepository;

	public DeleteTodoCommandHandler(ITodoRepository todoRepository, IUserRepository userRepository)
	{
		_todoRepository = todoRepository;
		_userRepository = userRepository;
	}

	public async Task<Unit> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(command.UserId) is not User user)
		{
			throw new ApiException("Not Found", "User does not exist.", nameof(DeleteTodoCommand),
				HttpStatusCode.NotFound);
		}

		if (await _todoRepository.GetTodoById(command.Id) is not Todo todo)
		{
			throw new ApiException("Not Found", "Todo does not exist.", nameof(DeleteTodoCommand), 
				HttpStatusCode.NotFound);
		}
		
		if (todo.UserId != user.Id)
		{
			throw new ApiException("Unauthorized", "You do not have access to this resource.",
				nameof(DeleteTodoCommand), HttpStatusCode.Unauthorized);
		}


		await _todoRepository.DeleteTodo(todo.Id);

		return Unit.Value;
	}
}