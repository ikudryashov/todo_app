using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly IUserRepository _userRepository;

	public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
		_userRepository = userRepository;
	}

	public AuthenticationResult SignUp(string firstName, string lastName, string email, string password)
	{
		//ensure the user does not exist
		if (_userRepository.GetUserByEmail(email) is not null)
		{
			throw new Exception("user with given email already exists");
		}

		//create user
		var user = new User()
		{
			Id = Guid.NewGuid(),
			FirstName = firstName,
			LastName = lastName,
			Email = email,
			Password = password
		};
		
		//persist user
		_userRepository.CreateUser(user);
		
		//generate token
		var token = _jwtTokenGenerator.GenerateToken(user);
		
		return new AuthenticationResult(user, token);
	}

	public AuthenticationResult LogIn(string email, string password)
	{
		//ensure the user exists
		if (_userRepository.GetUserByEmail(email) is not User user)
		{
			throw new Exception("user with given email does not exist");
		}

		//validate password
		if (user.Password != password)
		{
			throw new Exception("given password was incorrect");
		}
		
		//generate token
		var token = _jwtTokenGenerator.GenerateToken(user);
		
		return new AuthenticationResult(
			user, 
			token);
	}
}