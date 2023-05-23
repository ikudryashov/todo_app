using MediatR;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Common.Interfaces.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Exceptions.User.Authentication;

namespace TodoApp.Application.Authentication.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthenticationResult>
{
	private readonly IUserRepository _userRepository;
 	private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
 
 	public RefreshCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IDateTimeProvider dateTimeProvider)
 	{
 		_userRepository = userRepository;
 		_jwtTokenGenerator = jwtTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }
	public async Task<AuthenticationResult> Handle(RefreshCommand command, CancellationToken cancellationToken)
	{
		if (_userRepository.GetUserByRefreshToken(command.RefreshToken) is not User user ||
		    command.RefreshToken != user.RefreshToken ||
		    _dateTimeProvider.UtcNow > user.RefreshTokenExpiryDate)
		{
			throw new InvalidCredentialsException();
		}

		var token = _jwtTokenGenerator.GenerateToken(user);
		var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

		user.RefreshToken = refreshToken.Token;
		user.RefreshTokenExpiryDate = refreshToken.ExpiryDate;
		
		_userRepository.UpdateUser(user);

		return new AuthenticationResult(user, token);
	}
}