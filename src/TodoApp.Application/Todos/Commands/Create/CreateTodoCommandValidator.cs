using FluentValidation;

namespace TodoApp.Application.Todos.Commands.Create;

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
	public CreateTodoCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty();
		RuleFor(x => x.Title)
			.NotEmpty()
			.MinimumLength(1)
			.MaximumLength(100);
		RuleFor(x => x.Description)
			.MaximumLength(1000);
	}
}