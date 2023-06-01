using FluentValidation;

namespace TodoApp.Application.Authentication.Queries.LogIn;

public class LogInQueryValidator : AbstractValidator<LogInQuery>
{
	public LogInQueryValidator()
	{
		RuleFor(x => x.Email).NotEmpty();
		RuleFor(x => x.Password).NotEmpty();
	}
}