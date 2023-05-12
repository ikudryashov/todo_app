namespace TodoApp.Application.Services.Authentication;

public interface IAuthenticationService
{
	public AuthenticationResult SignUp(string firstName, string lastName, string email, string password);
	public AuthenticationResult LogIn(string email, string password);
}