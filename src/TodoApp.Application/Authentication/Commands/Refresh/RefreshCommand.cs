using MediatR;
using TodoApp.Application.Authentication.Common;

namespace TodoApp.Application.Authentication.Commands.Refresh;

public record RefreshCommand
(
	Guid UserId,
	string RefreshToken
) : IRequest<AuthenticationResult>;