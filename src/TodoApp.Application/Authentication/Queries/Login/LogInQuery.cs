using MediatR;
using TodoApp.Application.Authentication.Common;

namespace TodoApp.Application.Authentication.Queries.Login;

public record LogInQuery(
	string Email,
	string Password) : IRequest<AuthenticationResult>;