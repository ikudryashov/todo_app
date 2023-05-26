using FluentValidation;

namespace TodoApp.Application.Authentication.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
	public SignUpCommandValidator()
	{
		RuleFor(x => x.FirstName).NotEmpty();
		RuleFor(x => x.LastName).NotEmpty();
		RuleFor(x => x.Email).NotEmpty();
		RuleFor(x => x.Password).NotEmpty();
	}
}