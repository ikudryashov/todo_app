using FluentValidation;

namespace TodoApp.Application.Todos.Commands.Delete;

public class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
	public DeleteTodoCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.UserId).NotEmpty();
	}
}