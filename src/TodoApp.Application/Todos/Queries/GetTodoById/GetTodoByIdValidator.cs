using FluentValidation;

namespace TodoApp.Application.Todos.Queries.GetTodoById;

public class GetTodoByIdValidator : AbstractValidator<GetTodoByIdQuery>
{
	public GetTodoByIdValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.UserId).NotEmpty();
	}
}