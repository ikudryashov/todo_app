using FluentValidation;

namespace TodoApp.Application.Users.Queries;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
	public GetUserQueryValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.RequestId).NotEmpty();
	}
}