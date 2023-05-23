using MediatR;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Exceptions.User.Authentication;

namespace TodoApp.Application.Authentication.Queries.Login;

public class LogInQueryHandler : IRequestHandler<LogInQuery, AuthenticationResult>
{
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IUserRepository _userRepository;

	public LogInQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
		_userRepository = userRepository;
		_passwordHasher = passwordHasher;
	}

	public async Task<AuthenticationResult> Handle(LogInQuery query, CancellationToken cancellationToken)
	{
		if (_userRepository.GetUserByEmail(query.Email) is not User user ||
		    !_passwordHasher.VerifyPassword(query.Password, user.Password, user.Salt))
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