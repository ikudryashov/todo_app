using TodoApp.Application.Services.Authentication.Common;

namespace TodoApp.Application.Services.Authentication.Queries;

public interface IAuthenticationQueryService
{
	public AuthenticationResult LogInQuery(string email, string password);
}