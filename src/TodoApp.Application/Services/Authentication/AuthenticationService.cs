using TodoApp.Application.Common.Interfaces.Authentication;

namespace TodoApp.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
	private readonly IJwtTokenGenerator _jwtTokenGenerator;

	public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
	}

	public AuthenticationResult SignUp(string firstName, string lastName, string email, string password)
	{
		//ensure the user does not exist
		
		//create user
		
		//generate token
		var userId = Guid.NewGuid();
		var token = _jwtTokenGenerator.GenerateToken(userId, firstName, lastName);
		
		return new AuthenticationResult(userId, firstName, lastName, email, token);
	}

	public AuthenticationResult LogIn(string email, string password)
	{
		//ensure the user exists
		
		//validate password
		
		//generate token
		var userId = Guid.NewGuid();
		var token = _jwtTokenGenerator.GenerateToken(userId, "firstName", "lastName");
		
		return new AuthenticationResult(
			userId, 
			"firstName", 
			"lastName", 
			email, 
			token);
	}
}