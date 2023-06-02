using FluentValidation;

namespace TodoApp.Application.Todos.Queries.GetTodos;

public class GetTodosQueryValidator : AbstractValidator<GetTodosQuery>
{
	public GetTodosQueryValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
	}
}