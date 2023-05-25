using MediatR;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Common.Interfaces.Services;
using TodoApp.Domain.Exceptions.RefreshToken.Authentication;
using TodoApp.Domain.Exceptions.User;

namespace TodoApp.Application.Authentication.Commands.LogOut;

public class LogOutCommandHandler : IRequestHandler<LogOutCommand>
{
	private readonly IRefreshTokenRepository _refreshTokenRepository;
	private readonly IUserRepository _userRepository;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ICredentialsHasher _credentialsHasher;

	public LogOutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IDateTimeProvider dateTimeProvider, ICredentialsHasher credentialsHasher)
	{
		_refreshTokenRepository = refreshTokenRepository;
		_userRepository = userRepository;
		_dateTimeProvider = dateTimeProvider;
		_credentialsHasher = credentialsHasher;
	}

	public async Task Handle(LogOutCommand command, CancellationToken cancellationToken)
	{
		var refreshToken = await _refreshTokenRepository.GetRefreshTokenByUserId(command.UserId);

		if (refreshToken is null)
		{
			throw new InvalidTokenException();
		}
		
		refreshToken.IsValid = false;

		await _refreshTokenRepository.UpdateRefreshToken(refreshToken);
	}
}