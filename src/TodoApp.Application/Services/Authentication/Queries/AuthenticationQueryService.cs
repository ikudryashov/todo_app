using TodoApp.Application.Common.Exceptions.Authentication;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Services.Authentication.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services.Authentication.Queries;

public class AuthenticationQueryService : IAuthenticationQueryService
{
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly IUserRepository _userRepository;

	public AuthenticationQueryService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
		_userRepository = userRepository;
	}

	public AuthenticationResult LogInQuery(string email, string password)
	{

		if (_userRepository.GetUserByEmail(email) is not User user ||
		    user.Password != password)
		{
			throw new InvalidCredentialsException();
		}

		//generate token
		var token = _jwtTokenGenerator.GenerateToken(user);
		
		return new AuthenticationResult(
			user, 
			token);
	}
}