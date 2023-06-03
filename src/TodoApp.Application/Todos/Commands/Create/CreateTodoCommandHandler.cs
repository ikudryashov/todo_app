using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Todos.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.Commands.Create;

public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, TodoResult>
{
	private readonly ITodoRepository _todoRepository;
	private readonly IUserRepository _userRepository;

	public CreateTodoCommandHandler(ITodoRepository todoRepository, IUserRepository userRepository)
	{
		_todoRepository = todoRepository;
		_userRepository = userRepository;
	}

	public async Task<TodoResult> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(command.UserId) is null)
		{
			throw new ApiException("Not Found", "User does not exist.", HttpStatusCode.NotFound);
		}

		var todo = new Todo
		{
			Id = Guid.NewGuid(),
			UserId = command.UserId,
			Title = command.Title,
			Description = command.Description,
			DueDate = command.DueDate
		};

		await _todoRepository.CreateTodo(todo);

		return new TodoResult(todo.Id, todo.Title, todo.Description, todo.DueDate);
	}
}