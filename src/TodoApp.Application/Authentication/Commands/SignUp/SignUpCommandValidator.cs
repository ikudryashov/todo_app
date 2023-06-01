using FluentValidation;

namespace TodoApp.Application.Authentication.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
	public SignUpCommandValidator()
	{
		RuleFor(x => x.FirstName)
			.NotEmpty()
			.MaximumLength(30);
		RuleFor(x => x.LastName)
			.NotEmpty()
			.MaximumLength(30);
		RuleFor(x => x.Email)
			.NotEmpty()
			.MaximumLength(40)
			.EmailAddress();
		RuleFor(x => x.Password)
			.NotEmpty()
			.MinimumLength(14)
			.MaximumLength(30)
			.Must(HaveSpecialChar).WithMessage("Password must contain at least 1 special character.");
	}

	private bool HaveSpecialChar(string pwd)
	{
		return pwd.Any(c => !Char.IsLetterOrDigit(c));
	}
}