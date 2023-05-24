using MediatR;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Common.Interfaces.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Exceptions.RefreshToken.Authentication;
using TodoApp.Domain.Exceptions.User;

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

		if (refreshToken is null 
		    || !refreshToken.IsValid
		    || !_credentialsHasher.Verify(command.RefreshToken, refreshToken.Token, refreshToken.Salt))
		{
			throw new InvalidTokenException();
		}

		if (refreshToken.ExpiryDate < _dateTimeProvider.UtcNow)
		{
			throw new ExpiredTokenException();
		}

		var user = await _userRepository.GetUserById(refreshToken.UserId);

		if (user is null)
		{
			throw new UserNotFoundException();
		}

		var jwtToken = _jwtTokenGenerator.GenerateToken(user);
		var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);
		var plaintextNewRefreshToken = newRefreshToken.Token;
		var hashedNewRefreshToken = _credentialsHasher.Hash(newRefreshToken.Token);

		newRefreshToken.Token = hashedNewRefreshToken.hash;
		newRefreshToken.Salt = hashedNewRefreshToken.salt;
		
		await _refreshTokenRepository.UpdateRefreshToken(newRefreshToken);

		newRefreshToken.Token = plaintextNewRefreshToken;

		return new AuthenticationResult(user, jwtToken, newRefreshToken.Token);
	}
}