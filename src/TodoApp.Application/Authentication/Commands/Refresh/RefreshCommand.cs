using MediatR;
using TodoApp.Application.Authentication.Common;

namespace TodoApp.Application.Authentication.Commands.Refresh;

public record RefreshCommand
(
	string RefreshToken
) : IRequest<AuthenticationResult>;