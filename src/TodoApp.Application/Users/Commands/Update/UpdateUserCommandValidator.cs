using FluentValidation;

namespace TodoApp.Application.Users.Commands.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.RequestId).NotEmpty();
		RuleFor(x => x.FirstName)
			.NotEmpty()
			.MinimumLength(1)
			.MaximumLength(30);
		RuleFor(x => x.LastName)
			.NotEmpty()
			.MinimumLength(1)
			.MaximumLength(30);
		RuleFor(x => x.Email)
			.MaximumLength(40)
			.EmailAddress();
	}
}