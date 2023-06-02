using FluentValidation;

namespace TodoApp.Application.Todos.Commands.Update;

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
	public UpdateTodoCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();
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