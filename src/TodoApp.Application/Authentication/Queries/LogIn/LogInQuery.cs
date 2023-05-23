using MediatR;
using TodoApp.Application.Authentication.Common;

namespace TodoApp.Application.Authentication.Queries.LogIn;

public record LogInQuery(
	string Email,
	string Password) : IRequest<AuthenticationResult>;