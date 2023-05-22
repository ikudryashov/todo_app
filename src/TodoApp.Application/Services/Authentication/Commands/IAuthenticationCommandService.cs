using TodoApp.Application.Services.Authentication.Common;

namespace TodoApp.Application.Services.Authentication.Commands;

public interface IAuthenticationCommandService
{
	public AuthenticationResult SignUpCommand(string firstName, string lastName, string email, string password);
}