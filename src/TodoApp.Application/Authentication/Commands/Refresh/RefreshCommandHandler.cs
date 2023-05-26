using System.Net;
using MediatR;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Common.Interfaces.Services;

namespace TodoApp.Application.Authentication.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthenticationResult>
{
	private readonly IUserRepository _userRepository;
	private readonly IRefreshTokenRepository _refreshTokenRepository;
 	private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICredentialsHasher _credentialsHasher;
 
 	public RefreshCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IDateTimeProvider dateTimeProvider, IRefreshTokenRepository refreshTokenRepository, ICredentialsHasher credentialsHasher)
 	{
 		_userRepository = userRepository;
 		_jwtTokenGenerator = jwtTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _credentialsHasher = credentialsHasher;
    }
	public async Task<AuthenticationResult> Handle(RefreshCommand command, CancellationToken cancellationToken)
	{
		var refreshToken = await _refreshTokenRepository.GetRefreshTokenByUserId(command.UserId);

		if (refreshToken is null)
		{
			throw new ApiException("Failed to authenticate.",
				"Refresh token not found.", HttpStatusCode.Unauthorized);
		}

		if (!refreshToken.IsValid
		    || !_credentialsHasher.Verify(command.RefreshToken, refreshToken.Token, refreshToken.Salt))
		{
			throw new ApiException("Failed to authenticate.",
				"Refresh token is invalid.", HttpStatusCode.BadRequest);
		}

		if (refreshToken.ExpiryDate < _dateTimeProvider.UtcNow)
		{
			throw new ApiException("Failed to authenticate.",
				"Refresh token expired, please log in.", HttpStatusCode.Unauthorized);
		}

		var user = await _userRepository.GetUserById(refreshToken.UserId);

		if (user is null)
		{
			throw new ApiException("Failed to authenticate.",
				"User not found.", HttpStatusCode.Unauthorized);
		}

		//generate tokens
		var accessToken = _jwtTokenGenerator.GenerateToken(user);
		var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);
		
		//form result 
		var result = new AuthenticationResult(user.Id, accessToken, newRefreshToken.Token);
		
		//update the refresh token in the persistence layer
		var hashedNewRefreshToken = _credentialsHasher.Hash(newRefreshToken.Token);
		newRefreshToken.Token = hashedNewRefreshToken.hash;
		newRefreshToken.Salt = hashedNewRefreshToken.salt;
		await _refreshTokenRepository.UpdateRefreshToken(newRefreshToken);

		return result;
	}
}