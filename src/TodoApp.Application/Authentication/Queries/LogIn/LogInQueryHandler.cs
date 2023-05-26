using System.Net;
using MediatR;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

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
			throw new ApiException("Failed to authenticate.",
				"User does not exist or provided invalid credentials.",
				HttpStatusCode.BadRequest);
		}

		//generate tokens
		var accessToken = _jwtTokenGenerator.GenerateToken(user);
		var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);

		//form result
		var result = new AuthenticationResult(user.Id, accessToken, refreshToken.Token);
		
		//update refresh token
		var hashedRefreshToken = _credentialsHasher.Hash(refreshToken.Token);
		refreshToken.Token = hashedRefreshToken.hash;
		refreshToken.Salt = hashedRefreshToken.salt;
		await _refreshTokenRepository.UpdateRefreshToken(refreshToken);

		return result;
	}
}