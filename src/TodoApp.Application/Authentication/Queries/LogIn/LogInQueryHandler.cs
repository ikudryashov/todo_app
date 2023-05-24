using MediatR;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Exceptions.User.Authentication;

namespace TodoApp.Application.Authentication.Queries.LogIn;

public class LogInQueryHandler : IRequestHandler<LogInQuery, AuthenticationResult>
{
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly ICredentialsHasher _credentialsHasher;
	private readonly IUserRepository _userRepository;
	private readonly IRefreshTokenRepository _refreshTokenRepository;

	public LogInQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, ICredentialsHasher credentialsHasher, IRefreshTokenRepository refreshTokenRepository)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
		_userRepository = userRepository;
		_credentialsHasher = credentialsHasher;
		_refreshTokenRepository = refreshTokenRepository;
	}

	public async Task<AuthenticationResult> Handle(LogInQuery query, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserByEmail(query.Email) is not User user ||
		    !_credentialsHasher.Verify(query.Password, user.Password, user.Salt))
		{
			throw new InvalidCredentialsException();
		}

		//generate tokens
		var accessToken = _jwtTokenGenerator.GenerateToken(user);
		var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);
		
		var plaintextRefreshToken = refreshToken.Token;
		var hashedRefreshToken = _credentialsHasher.Hash(refreshToken.Token);

		refreshToken.Token = hashedRefreshToken.hash;
		refreshToken.Salt = hashedRefreshToken.salt;
		
		await _refreshTokenRepository.UpdateRefreshToken(refreshToken);

		refreshToken.Token = plaintextRefreshToken;
		
		return new AuthenticationResult(
			user, 
			accessToken,
			refreshToken.Token);
	}
}