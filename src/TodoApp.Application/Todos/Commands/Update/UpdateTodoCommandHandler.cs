using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Commands.Update;

public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, Unit>
{
	private readonly IUserRepository _userRepository;
	private readonly ITodoRepository _todoRepository;

	public UpdateTodoCommandHandler(IUserRepository userRepository, ITodoRepository todoRepository)
	{
		_userRepository = userRepository;
		_todoRepository = todoRepository;
	}

	public async Task<Unit> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(command.UserId) is not User user)
		{
			throw new ApiException("Not Found", "User does not exist.", HttpStatusCode.NotFound);
		}

		if (await _todoRepository.GetTodoById(command.Id) is not Todo todo)
		{
			throw new ApiException("Not Found", "Todo does not exist.", HttpStatusCode.NotFound);
		}

		if (todo.UserId != user.Id)
		{
			throw new ApiException("Unauthorized", "You do not have access to this resource.",
				HttpStatusCode.Unauthorized);
		}

		var updatedTodo = new Todo
		{
			Id = todo.Id,
			UserId = todo.UserId,
			Title = command.Title,
			Description = command.Description,
			DueDate = command.DueDate,
			IsComplete = command.IsComplete
		};

		await _todoRepository.UpdateTodo(updatedTodo);

		return Unit.Value;
	}
}