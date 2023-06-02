using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;

namespace TodoApp.Application.Authentication.Commands.LogOut;

public class LogOutCommandHandler : IRequestHandler<LogOutCommand>
{
	private readonly IRefreshTokenRepository _refreshTokenRepository;

	public LogOutCommandHandler(IRefreshTokenRepository refreshTokenRepository)
	{
		_refreshTokenRepository = refreshTokenRepository;
	}

	public async Task Handle(LogOutCommand command, CancellationToken cancellationToken)
	{
		var refreshToken = await _refreshTokenRepository.GetRefreshTokenByUserId(command.UserId);

		if (refreshToken is null)
		{
			throw new ApiException("Failed to authenticate.",
				"Refresh token not found.", HttpStatusCode.NotFound);
		}
		
		refreshToken.IsValid = false;

		await _refreshTokenRepository.UpdateRefreshToken(refreshToken);
	}
}