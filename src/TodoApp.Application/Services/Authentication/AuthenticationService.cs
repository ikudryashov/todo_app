namespace TodoApp.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
	public AuthenticationResult SignUp(string firstName, string lastName, string email, string password)
	{
		return new AuthenticationResult(Guid.NewGuid(), firstName, lastName, email, "token");
	}

	public AuthenticationResult LogIn(string email, string password)
	{
		return new AuthenticationResult(Guid.NewGuid(), "firstName", "lastName", email, "token");
	}
}