using MediatR;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Common.Interfaces.Services;
using TodoApp.Domain.Exceptions.RefreshToken.Authentication;

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