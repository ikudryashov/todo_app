using TodoApp.Domain.Entities;

namespace TodoApp.Application.Authentication.Common;

public record AuthenticationResult(
		Guid UserId,
		string AccessToken,
		string RefreshToken
);