using MediatR;
using TodoApp.Application.Authentication.Common;

namespace TodoApp.Application.Authentication.Commands.SignUp;

public record SignUpCommand(
	string FirstName,
	string LastName,
	string Email,
	string Password) : IRequest<AuthenticationResult>;