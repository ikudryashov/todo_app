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
	private readonly IUserRepository _userRepository;

	public LogInQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
		_userRepository = userRepository;
	}

	public async Task<AuthenticationResult> Handle(LogInQuery query, CancellationToken cancellationToken)
	{
		if (_userRepository.GetUserByEmail(query.Email) is not User user ||
		    user.Password != query.Password)
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